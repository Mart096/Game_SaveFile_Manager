using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Games_SaveFiles_Manager.MediatorComms
{
    //Solution proposed by Sacha Barber
    //https://www.codeproject.com/articles/35277/mvvm-mediator-pattern
    public sealed class Mediator
    {
        #region enums
        /// <summary>
        /// Available cross ViewModel messages
        /// </summary>
        public enum ViewModelMessages
        {
            ProfileListUpdated = 1,
            GameListUpdated = 2
        };
        #endregion

        #region fields
        static readonly Mediator instance = new Mediator();
        private volatile object locker = new object();

        MultiDictionary<ViewModelMessages, Action<Object>> internalList = new MultiDictionary<ViewModelMessages, Action<object>>();
        #endregion

        #region Ctors
        static Mediator()
        {

        }

        private Mediator()
        {

        }
        #endregion

        #region Properties
        public static Mediator Instance
        {
            get{ return instance; }
        }

        #endregion

        #region Methods
        public void Register(Action<object> callback, ViewModelMessages message)
        {
            internalList.AddValue(message, callback);
        }

        /// <summary>
        /// Notify all subscribers that are registered to the specific message
        /// </summary>
        /// <param name="message">The message that will be in notification</param>
        /// <param name="args">The arguments for the message</param>
        public void NotifyColleagues (ViewModelMessages message, object args)
        {
            if (internalList.ContainsKey(message))
            {
                foreach(Action<object> callback in internalList[message])
                {
                    callback(args);
                }
            }
        }

        #endregion

    }
}
