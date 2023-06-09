﻿// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

using System.Reflection;

namespace SurveyApp.App;

/// <summary>Represents an entity base.</summary>
public abstract class EntityBase
{
  /// <summary>Gets an object that represents a collection of related entities.</summary>
  public virtual string[] Relations() => Array.Empty<string>();

  /// <summary>Compares this entity.</summary>
  /// <param name="updatedEntity">An object that represents an entity from which this entity should be compared.</param>
  /// <returns>An object that represents a collection of updated properties.</returns>
  protected string[] Compare(object updatedEntity)
  {
    string[] updatingProperties = GetUpdatingProperties();
    string[] updatedProperties  = updatingProperties;

    return Compare(updatedEntity, updatedProperties, updatingProperties);
  }

  /// <summary>Compare this entity.</summary>
  /// <param name="newEntity">An object that represents an entity from which this entity should be compared.</param>
  /// <param name="propertiesToUpdate">An object that represents a collection of properties to update.</param>
  /// <returns>An object that represents a collection of updated properties.</returns>
  protected string[] Compare(object newEntity, string[] propertiesToUpdate) =>
    Compare(newEntity, propertiesToUpdate, GetUpdatingProperties());

  /// <summary>Updates this entity.</summary>
  /// <param name="newEntity">An object that represents an entity from which this entity should be updated.</param>
  /// <param name="propertiesToUpdate">An object that represents a collection of properties to update.</param>
  /// <param name="updatingProperties">An object that represents a collection of properties that can be updated.</param>
  /// <returns></returns>
  protected virtual string[] Compare(object newEntity, string[] propertiesToUpdate, string[] updatingProperties)
  {
    List<string> updatedProperties = new(propertiesToUpdate.Length);

    foreach (var property in propertiesToUpdate)
    {
      if (updatingProperties.Contains(property))
      {
        PropertyInfo originalProperty = GetType().GetProperty(property)!;
        PropertyInfo updatedProperty  = newEntity.GetType().GetProperty(property)!;

        object? originalValue = originalProperty.GetValue(this);
        object? updatedValue  = updatedProperty.GetValue(newEntity);

        if (!object.Equals(originalValue, updatedValue))
        {
          updatedProperties.Add(property);
        }
      }
    }

    return updatedProperties.ToArray();
  }

  /// <summary>Creates a copy of an entity.</summary>
  /// <param name="entity">An object that represents an entity to copy.</param>
  /// <returns>An object that represents an instance of an entity copy.</returns>
  /// <exception cref="System.NotSupportedException">Throws if there is no such entity.</exception>
  public static T2 Create<T1, T2>(T1 entity) where T2 : EntityBase, T1
  {
    ArgumentNullException.ThrowIfNull(entity);

    if (entity.GetType() == typeof(T2))
    {
      return (T2)entity;
    }

    return (T2)typeof(T2).GetConstructor(new[] { typeof(T1) })!
                         .Invoke(new object[] { entity! });
  }

  private string[] GetUpdatingProperties()
  {
    PropertyInfo[]    allProperties = GetType().GetProperties();
    List<string> updatingProperties = new(allProperties.Length);

    for (int i = 0; i < allProperties.Length; i++)
    {
      if (allProperties[i].CanWrite)
      {
        updatingProperties.Add(allProperties[i].Name);
      }
    }

    return updatingProperties.ToArray();
  }
}
