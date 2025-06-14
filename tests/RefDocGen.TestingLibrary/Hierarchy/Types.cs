namespace RefDocGen.TestingLibrary.Hierarchy
{
    /// <summary>
    /// IChild interface.
    /// </summary>
    interface IChild
    {
        /// <summary>
        /// IChild Print the object.
        /// </summary>
        /// <param name="obj">Object to print.</param>
        void Print(object obj);
    }

    /// <summary>
    /// Parent class.
    /// </summary>
    public class Parent
    {
        /// <summary>
        /// Handle the object.
        /// </summary>
        /// <param name="obj">The object to handle.</param>
        /// <returns>Boolean indicating whether the handling was successful.</returns>
        public virtual bool Handle(object obj)
        {
            return false;
        }

        public virtual void Print(object obj) { } // no documentation
    }

    /// <inheritdoc/>
    internal class Child : Parent, IChild
    {
        /// <inheritdoc/>
        public override bool Handle(object obj)
        {
            return false;
        }

        /// <inheritdoc/>
        public override void Print(object obj)
        {
            throw new NotImplementedException();
        }
    }

    /// <inheritdoc cref="IChild.Print(object)"/>
    internal class ChildChild : Child
    {
        /// <inheritdoc/>
        public override bool Handle(object obj)
        {
            return false;
        }

        /// <summary>
        /// ChildChild Print
        /// </summary>
        /// <param name="obj">
        ///   <inheritdoc path="/param[@name='obj']"/>
        /// </param>
        /// <remarks>
        /// <inheritdoc/>
        /// </remarks>
        public override void Print(object obj)
        {
            throw new NotImplementedException();
        }


        /// <inheritdoc cref="IChild.Print(object)"/>
        public void PrintData()
        {

        }
    }

    /// <summary>
    /// Before parent.
    /// <inheritdoc cref="Parent"/> 
    /// After parent.
    /// </summary>
    internal class ChildChildChild : ChildChild
    {
        /// <inheritdoc/>
        public override void Print(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
