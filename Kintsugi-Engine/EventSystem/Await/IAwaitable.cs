namespace Kintsugi.EventSystem.Await
{
    /// <summary>
    /// Something that can be finished and waited on.
    /// </summary>
    public interface IAwaitable
    {
        /// <summary>
        /// Whether it needs to be waited on any longer.
        /// </summary>
        public abstract bool IsFinished();
    }
}
