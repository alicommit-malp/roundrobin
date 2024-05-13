using System.Collections.Generic;

namespace RoundRobin
{
    /// <summary>
    /// Provides extension methods for working with a circular linked list.
    /// </summary>
    /// <remarks>
    /// A circular linked list is a linked list where the last node points back to the first node.
    /// The extension methods in this class allow you to easily navigate through the circular linked list.
    /// </remarks>
    public static class CircularLinkedList
    {
        /// <summary>
        /// Moves to the next node in the circular linked list or returns the first node if the current node is the last node.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the linked list.</typeparam>
        /// <param name="current">The current node in the linked list.</param>
        /// <returns>The next node in the linked list or the first node if the current node is the last node.</returns>
        public static LinkedListNode<T> NextOrFirst<T>(this LinkedListNode<T> current)
        {
            return current.Next ?? current.List?.First;
        }

        /// <summary>
        /// Moves to the previous node in the circular linked list or returns the last node if the current node is the first node.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the linked list.</typeparam>
        /// <param name="current">The current node in the linked list.</param>
        /// <returns>The previous node in the linked list or the last node if the current node is the first node.</returns>
        public static LinkedListNode<T> PreviousOrLast<T>(this LinkedListNode<T> current)
        {
            return current.Previous ?? current.List?.Last;
        }
    }
}