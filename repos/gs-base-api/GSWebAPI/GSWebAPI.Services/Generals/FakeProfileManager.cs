using System;
using System.Collections.Generic;
using GSWebAPI.GSWebAPI.Domain.Models.General;
using GSWebAPI.GSWebAPI.Infrastructure.Repositories.Generals;

namespace GSWebAPI.GSWebAPI.Services.Generals
{
    /// <summary>
    /// Handles the fake profiles.
    /// </summary>
    public class FakeProfileManager
    {
        private readonly string _connectionString;
        private readonly string _connectionStringRead;

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeProfileManager"/> class.
        /// Manages the fake profiles.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="connectionStringRead">The connection string to read.</param>
        public FakeProfileManager(string connectionString, string connectionStringRead)
        {
            _connectionString = connectionString;
            _connectionStringRead = connectionStringRead;
        }

        /// <summary>
        /// Returns a fake profile id.
        /// </summary>
        /// <returns>Returns the ID of a fake profile.</returns>
        public string GetFakeProfileIdRandom()
        {
            // First, select the fake profiles.
            List<FakeProfileModel> fakeProfileModelList = new List<FakeProfileModel>();
            FakeProfileDAO fakeProfileDAO = new FakeProfileDAO();
            FakeProfileModel fakeProfileModel = new FakeProfileModel();
            fakeProfileModelList = fakeProfileDAO.Read(_connectionStringRead, fakeProfileModel, false, false);

            // Now take a random id
            Random random = new Random();
            int randomNumber = random.Next(0, fakeProfileModelList.Count);

            return fakeProfileModelList[randomNumber].UserId;
        }
    }
}