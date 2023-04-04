﻿// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace Survey.Infrastructure.Repositories.Test
{
  using System;

  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.DependencyInjection;

  using Survey.Domain.Entities;
  using Survey.Domain.Repositories;
  using Survey.Infrastructure.Entities;
  using Survey.Test.Integration;

  [TestClass]
  public sealed class SurveyRepositoryTest : IntegrationTestBase
  {
#pragma warning disable CS8618
    private ISurveyRepository _surveyRepository;
#pragma warning restore CS8618

    protected override void InitializeInternal()
    {
      _surveyRepository = ServiceProvider.GetRequiredService<ISurveyRepository>();
    }

    [TestMethod]
    public async Task AddSurveyAsync_Should_Create_New_Survey()
    {
      var controlSurveyEntity = SurveyRepositoryTest.GenerateTestSurveyEntity();

      await _surveyRepository.AddSurveyAsync(controlSurveyEntity, CancellationToken);

      var actualSurveyEntity =
        await DbContext.Set<SurveyEntity>()
                       .AsNoTracking()
                       .Where(entity => entity.SurveyId == controlSurveyEntity.SurveyId)
                       .SingleOrDefaultAsync(CancellationToken);

      Assert.IsNotNull(actualSurveyEntity);

      SurveyRepositoryTest.AreEqual(controlSurveyEntity, actualSurveyEntity);
      IsDetached(controlSurveyEntity);
    }

    [TestMethod]
    public async Task GetSurveyAsync_Should_Return_Detached_Entity()
    {
      var controlSurveyEntity = SurveyRepositoryTest.GenerateTestSurveyEntity();
      var controlSurveyEntityEntry = DbContext.Add(controlSurveyEntity);

      await DbContext.SaveChangesAsync(CancellationToken);

      controlSurveyEntityEntry.State = EntityState.Detached;

      var actualSurveyEntity =
        await _surveyRepository.GetSurveyAsync(
          controlSurveyEntity.SurveyId, CancellationToken);

      Assert.IsNotNull(actualSurveyEntity);

      SurveyRepositoryTest.AreEqual(controlSurveyEntity, actualSurveyEntity);
      IsDetached(controlSurveyEntity);
    }

    [TestMethod]
    public async Task GetSurveyAsync_Should_Return_Null()
    {
      var controlSurveyEntity = SurveyRepositoryTest.GenerateTestSurveyEntity();
      var controlSurveyEntityEntry = DbContext.Add(controlSurveyEntity);

      await DbContext.SaveChangesAsync(CancellationToken);

      controlSurveyEntityEntry.State = EntityState.Detached;

      var actualSurveyEntity =
        await _surveyRepository.GetSurveyAsync(
          Guid.NewGuid(), CancellationToken);

      Assert.IsNull(actualSurveyEntity);
    }

    [TestMethod]
    public async Task GetSurveysAsync_Should_Return_Detached_Entities()
    {
      var controlSurveyEntityCollection =
        SurveyRepositoryTest.GenerateTestSurveyEntityCollection(5);

      await SaveChangesAsync(controlSurveyEntityCollection, CancellationToken);

      var actualSurveyEntityCollection =
        await _surveyRepository.GetSurveysAsync(CancellationToken);

      Assert.IsNotNull(actualSurveyEntityCollection);

      SurveyRepositoryTest.AreEqual(controlSurveyEntityCollection, actualSurveyEntityCollection);
      AreDetached(controlSurveyEntityCollection);
    }

    private static SurveyEntity GenerateTestSurveyEntity() => new()
    {
      SurveyId = Guid.NewGuid(),
      Name = Guid.NewGuid().ToString(),
      Description = Guid.NewGuid().ToString(),
    };

    private static SurveyEntity[] GenerateTestSurveyEntityCollection(int surveys)
    {
      var surveyEntityCollection = new SurveyEntity[surveys];

      for (int i = 0; i < surveys; i++)
      {
        surveyEntityCollection[i] = SurveyRepositoryTest.GenerateTestSurveyEntity();
      }

      return surveyEntityCollection;
    }

    private static void AreEqual(ISurveyEntity control, ISurveyEntity actual)
    {
      Assert.AreEqual(control.SurveyId, actual.SurveyId);
      Assert.AreEqual(control.Name, actual.Name);
      Assert.AreEqual(control.Description, actual.Description);
    }

    private static void AreEqual(ISurveyEntity[] control, ISurveyEntity[] actual)
    {
      Assert.AreEqual(control.Length, actual.Length);

      for (int i = 0; i < control.Length; i++)
      {
        SurveyRepositoryTest.AreEqual(control[i], actual[i]);
      }
    }

    private void IsDetached(SurveyEntity entity)
    {
      var entry = DbContext.Entry(entity);

      Assert.AreEqual(EntityState.Detached, entry.State);
    }

    private void AreDetached(SurveyEntity[] collection)
    {
      for (int i = 0; i < collection.Length; i++)
      {
        IsDetached(collection[i]);
      }
    }

    private async Task SaveChangesAsync(
      SurveyEntity[] controlSurveyEntityCollection, CancellationToken cancellationToken)
    {
      DbContext.AddRange(controlSurveyEntityCollection);

      await DbContext.SaveChangesAsync(CancellationToken);

      for (int i = 0; i < controlSurveyEntityCollection.Length; i++)
      {
        DbContext.Entry(controlSurveyEntityCollection[i]).State = EntityState.Detached;
      }
    }
  }
}