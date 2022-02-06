using System;
using Microsoft.AspNetCore.Mvc.Razor;

namespace PersonalWebsite.Views;

/// <summary>
/// A guard to let the views be free of Model nullability issues.
/// </summary>
/// <typeparam name="TModel">The type of a model</typeparam>
public abstract class NonNullableModelView<TModel>: RazorPage<TModel>
{
    protected TModel NNModel => base.Model ?? throw new InvalidOperationException("Model is null");
}
