﻿// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace Survey.Infrastructure.Survey
{
  using Microsoft.EntityFrameworkCore;

  using SurveyApp.Domain.Survey;

  /// <summary>Provides a simple API to the storage of the <see cref="SurveyApp.ApplicationCore.Entities.ISurveyEntity"/>.</summary>
  public sealed class SurveyRepository : ISurveyRepository
  {
    private readonly DbContext _dbContext;

    /// <summary>Initializes a new instance of the <see cref="Survey.Infrastructure.Survey.SurveyRepository"/> class.</summary>
    /// <param name="dbContext">An object that represents a session with the database and can be used to query and save instances of your entities.</param>
    public SurveyRepository(DbContext dbContext)
    {
      _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    /// <summary>Saves a new survey entity to the storage.</summary>
    /// <param name="surveyEntity">An object that represents a survey entity.</param>
    /// <param name="cancellationToken">An object that propagates notification that operations should be canceled.</param>
    /// <returns>An object that represents an asynchronous operation that produces a result at some time in the future. The result is an instance of the <see cref="ISurveyEntity"/>.</returns>
    public async Task<ISurveyEntity> AddSurveyAsync(ISurveyEntity surveyEntity, CancellationToken cancellationToken)
    {
      var entry = _dbContext.Add(new SurveyEntity(surveyEntity));

      await _dbContext.SaveChangesAsync(cancellationToken);

      entry.State = EntityState.Detached;

      return entry.Entity;
    }

    /// <summary>Updates a survey.</summary>
    /// <param name="surveyEntity">An object that represents a survey entity.</param>
    /// <param name="cancellationToken">An object that propagates notification that operations should be canceled.</param>
    /// <returns>An object that represents an asynchronous operation.</returns>
    public async Task UpdateSurveyAsync(ISurveyEntity surveyEntity, CancellationToken cancellationToken)
    {
      var entry = _dbContext.Update(new SurveyEntity(surveyEntity));

      await _dbContext.SaveChangesAsync(cancellationToken);

      entry.State = EntityState.Detached;
    }

    /// <summary>Deletes a survey.</summary>
    /// <param name="surveyIdentity">An object that represents a survey identity.</param>
    /// <param name="cancellationToken">An object that propagates notification that operations should be canceled.</param>
    /// <returns>An object that represents an asynchronous operation.</returns>
    public async Task DeleteSurveyAsync(ISurveyIdentity surveyIdentity, CancellationToken cancellationToken)
      => await _dbContext.Set<SurveyEntity>()
                         .Where(entity => entity.SurveyId == surveyIdentity.SurveyId)
                         .ExecuteDeleteAsync(cancellationToken);

    /// <summary>Gets a survey.</summary>
    /// <param name="surveyIdentity">An object that represents a survey identity.</param>
    /// <param name="cancellationToken">An object that propagates notification that operations should be canceled.</param>
    /// <returns>An object that represents an asynchronous operation that produces a result at some time in the future. The result is an instance of the <see cref="ISurveyEntity"/>.</returns>
    public async Task<ISurveyEntity?> GetSurveyAsync(ISurveyIdentity surveyIdentity, CancellationToken cancellationToken)
      => await _dbContext.Set<SurveyEntity>()
                         .AsNoTracking()
                         .Where(entity => entity.SurveyId == surveyIdentity.SurveyId)
                         .SingleOrDefaultAsync(cancellationToken);

    /// <summary>Gets a collection of surveys.</summary>
    /// <param name="cancellationToken">An object that propagates notification that operations should be canceled.</param>
    /// <returns>An object that represents an asynchronous operation that produces a result at some time in the future. The result is an collection of the <see cref="ISurveyEntity"/>.</returns>
    public async Task<ISurveyEntity[]> GetSurveysAsync(CancellationToken cancellationToken)
      => await _dbContext.Set<SurveyEntity>()
                         .AsNoTracking()
                         .OrderBy(entity => entity.SurveyId)
                         .ToArrayAsync(cancellationToken);
  }
}