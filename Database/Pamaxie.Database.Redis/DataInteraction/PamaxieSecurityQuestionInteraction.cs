using Newtonsoft.Json;
using Pamaxie.Data;
using Pamaxie.Database.Design;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pamaxie.Database.Server.DataInteraction
{
    /// <inheritdoc/>
    internal class PamaxieSecurityQuestionInteraction : PamaxieDataInteractionBase<ISecurityQuestions>, ISecurityQuestionInteraction
    {
        private PamaxieDatabaseDriver _owner;
        /// <summary>
        /// Passes through the database driver required for interaction with the database
        /// </summary>
        /// <param name="owner"></param>
        public PamaxieSecurityQuestionInteraction(PamaxieDatabaseDriver owner) : base(owner)
        {
            _owner = owner;
        }

        /// <inheritdoc/>
        public IPamaxieUser GetUser(ISecurityQuestions value)
        {
            throw new NotImplementedException();
        }
    }
}
