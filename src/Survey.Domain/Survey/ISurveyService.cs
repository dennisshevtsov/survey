﻿// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace SurveyApp.Domain.Survey
{
  /// <summary>Provides a simple API to the survey entity.</summary>
  public interface ISurveyService
  {
    /// <summary>Creates a new survey.</summary>
    /// <param name="surveyData">An object that represents survey data.</param>
    /// <param name="cancellationToken">An object that propagates notification that operations should be canceled.</param>
    /// <returns>An object that represents an asynchronous operation that produces a result at some time in the future. The result is an instance of the <see cref="Survey.Domain.Survey.ISurveyEntity"/>.</returns>
    public Task<ISurveyEntity> AddNewSurveyAsync(ISurveyData surveyData, CancellationToken cancellationToken);

    /// <summary>Updates a survey.</summary>
    /// <param name="surveyEntity">An object that represents a survey entity.</param>
    /// <param name="cancellationToken">An object that propagates notification that operations should be canceled.</param>
    /// <returns>An object that represents an asynchronous operation.</returns>
    public Task UpdateSurveyAsync(ISurveyEntity surveyEntity, CancellationToken cancellationToken);

    /// <summary>Deletes a survey.</summary>
    /// <param name="surveyIdentity">An object that represents a survey identity.</param>
    /// <param name="cancellationToken">An object that propagates notification that operations should be canceled.</param>
    /// <returns>An object that represents an asynchronous operation.</returns>
    public Task DeleteSurveyAsync(ISurveyIdentity surveyIdentity, CancellationToken cancellationToken);

    /// <summary>Gets a survey.</summary>
    /// <param name="surveyIdentity">An object that represents a survey identity.</param>
    /// <param name="cancellationToken">An object that propagates notification that operations should be canceled.</param>
    /// <returns>An object that represents an asynchronous operation that produces a result at some time in the future. The result is an instance of the <see cref="Survey.Domain.Survey.ISurveyEntity"/>.</returns>
    public Task<ISurveyEntity?> GetSurveyAsync(ISurveyIdentity surveyIdentity, CancellationToken cancellationToken);

    /// <summary>Gets a collection of surveys.</summary>
    /// <param name="cancellationToken">An object that propagates notification that operations should be canceled.</param>
    /// <returns>An object that represents an asynchronous operation that produces a result at some time in the future. The result is an collection of the <see cref="Survey.Domain.Survey.ISurveyEntity"/>.</returns>
    public Task<ISurveyEntity[]> GetSurveysAsync(CancellationToken cancellationToken);
  }
}