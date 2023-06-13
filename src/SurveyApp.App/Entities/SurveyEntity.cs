﻿// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace SurveyApp.Application.Entities
{
  using SurveyApp.Survey;

  /// <summary>Represents a survey entity.</summary>
  public sealed class SurveyEntity : ISurveyEntity
  {
    /// <summary>Initializes a new instance of the <see cref="SurveyApp.Application.Entities.SurveyEntity"/> class.</summary>
    /// <param name="surveyEntity">An object that represents a survey entity.</param>
    public SurveyEntity(ISurveyEntity surveyEntity)
    {
      SurveyId    = surveyEntity.SurveyId;
      Name        = surveyEntity.Name;
      Description = surveyEntity.Description;
    }

    /// <summary>Gets an object that represents an identity of a survey.</summary>
    public Guid SurveyId { get; set; }

    /// <summary>Gets an object that represents a name of a survey.</summary>
    public string Name { get; set; }

    /// <summary>Gets an object that represents a description of survey.</summary>
    public string Description { get; set; }
  }
}