using System.Collections.Generic;
using Pamaxie.Data;

namespace Pamaxie.Database.Design
{
    /// <summary>
    /// Interface that defines User interactions
    /// </summary>
    public interface ISecurityQuestionInteraction : IPamaxieDataInteractionBase<ISecurityQuestions>
    {
        /// <summary>
        /// Gets a <see cref="PamaxieUser"/> via their security questions
        /// </summary>
        /// <param name="value">The Security questions object</param>
        /// <returns><see cref="PamaxieUser"/> that owns these security questions</returns>
        public IPamaxieUser GetUser(ISecurityQuestions value);
    }
}