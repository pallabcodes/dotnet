using MediatR;
using Microsoft.Extensions.Logging;
using SaaSUsageBilling.Application.Abstractions;
using System.Text.Json;

namespace SaaSUsageBilling.Application.Behaviors;

public class IdempotencyBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<IdempotencyBehavior<TRequest, TResponse>> _logger;
    private readonly IIdempotencyStore _idempotencyStore;

    public IdempotencyBehavior(
        ILogger<IdempotencyBehavior<TRequest, TResponse>> logger,
        IIdempotencyStore idempotencyStore)
    {
        _logger = logger;
        _idempotencyStore = idempotencyStore;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        // Only apply to commands that have IdempotencyKey property
        var idempotencyKeyProperty = typeof(TRequest).GetProperty("IdempotencyKey");
        if (idempotencyKeyProperty == null)
        {
            return await next();
        }

        var idempotencyKey = (string?)idempotencyKeyProperty.GetValue(request);
        if (string.IsNullOrWhiteSpace(idempotencyKey))
        {
            _logger.LogWarning("Idempotency key is required but missing for {RequestType}",
                typeof(TRequest).Name);
            throw new InvalidOperationException("Idempotency key is required");
        }

        // Check if we've already processed this request
        var cachedResponse = await _idempotencyStore.GetResponseAsync(idempotencyKey, cancellationToken);
        if (cachedResponse != null)
        {
            _logger.LogInformation("Returning cached response for {RequestType} with key {IdempotencyKey}",
                typeof(TRequest).Name, idempotencyKey);

            try
            {
                return JsonSerializer.Deserialize<TResponse>(cachedResponse)!;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to deserialize cached response for key {IdempotencyKey}", idempotencyKey);
                // Fall through to reprocess
            }
        }

        // Record that we're processing this key (for basic existence check)
        await _idempotencyStore.RecordAsync(idempotencyKey, cancellationToken);

        var response = await next();

        // Store the response for future idempotent calls
        var serializedResponse = JsonSerializer.Serialize(response);
        await _idempotencyStore.StoreResponseAsync(idempotencyKey, serializedResponse, cancellationToken);

        _logger.LogInformation("Recorded idempotency key {IdempotencyKey} for {RequestType}",
            idempotencyKey, typeof(TRequest).Name);

        return response;
    }
}

