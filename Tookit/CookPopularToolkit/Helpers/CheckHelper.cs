/*
 *Description: CheckHelper
 *Author: Chance.zheng
 *Creat Time: 2023/8/8 15:39:05
 *.Net Version: 2.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace CookPopularToolkit
{
    public static class CheckHelper
    {
        #region Exception

        #region ArgumentException

        [DebuggerStepThrough]
        public static void ArgumentNullOrEmptyException(string value, string paramName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(paramName, "The parameter can not be either null or empty.");
            }

            if (value == "")
            {
                throw new ArgumentException("The parameter can not be either null or empty.", paramName);
            }
        }

        [DebuggerStepThrough]
        public static void ArgumentNullNorWhitespaceException(string value, string name)
        {
            if (value == null)
            {
                throw new ArgumentNullException(name, "The parameter can not be either null or empty or consist only of white space characters.");
            }

            if (value.Trim() == "")
            {
                throw new ArgumentException("The parameter can not be either null or empty or consist only of white space characters.", name);
            }
        }

        public static void ArgumentNullException(IEnumerable<object> objs, string paramName)
        {
            if (objs == null || objs.Count() <= 0)
                throw new ArgumentNullException(paramName, ExceptionMessage.GetArgumentNullOrEmptyExceptionMessage(paramName));
        }

        public static void ArgumentNullException(object? obj, string paramName)
        {
            if (obj == null)
                throw new ArgumentNullException(paramName);
        }

        public static void ArgumentNullExceptionWithMessage(object? obj, string message)
        {
            if (obj == null)
                throw new ArgumentNullException(message, default(Exception));
        }

        public static void ArgumentException(bool isThrowException, string errorMessage)
        {
            if (isThrowException)
                throw new ArgumentException(errorMessage);
        }

        [DebuggerStepThrough]
        public static void ArgumentDefaultException<T>(T obj, string name) where T : struct
        {
            if (default(T).Equals(obj))
            {
                throw new ArgumentException("The parameter must not be the default value.", name);
            }
        }

        #endregion

        #region FileNotFoundException

        public static void FileNotFoundException(string fileFullPath)
        {
            ArgumentNullOrEmptyException(fileFullPath, nameof(fileFullPath));

            if (!File.Exists(fileFullPath))
                throw new FileNotFoundException(ExceptionMessage.GetFileNotFoundExceptionMessage(fileFullPath));
        }

        public static void FileNotFoundException(bool isThrowException, string? errorMessage)
        {
            if (isThrowException)
                throw new FileNotFoundException(errorMessage);
        }

        #endregion

        #region OtherException

        [DebuggerStepThrough]
        public static void InvalidOperationException(ApartmentState requiredState, string message)
        {
            if (Thread.CurrentThread.GetApartmentState() != requiredState)
            {
                throw new InvalidOperationException(message);
            }
        }

        public static void NotSupportedException(string? errorMessage)
        {
            errorMessage = string.IsNullOrEmpty(errorMessage) ? new NotSupportedException().Message : errorMessage;
            throw new NotSupportedException(errorMessage);
        }

        public static void ArgumentOutOfRangeException(bool isThrowException, string paramName, object actualValue, string? errorMessage)
        {
            if (isThrowException)
                throw new ArgumentOutOfRangeException(paramName, actualValue, errorMessage);
        }

        public static void SystemException(bool isThrowException, string errorMessage, params string[] args)
        {
            if (isThrowException)
                throw new SystemException(string.Format(errorMessage, args));
        }

        public static void Exception(bool isThrowException, string errorMessage, params string[] args)
        {
            if (isThrowException)
                throw new Exception(string.Format(errorMessage, args));
        }

        #endregion

        #endregion

        #region Action

        public static void ActionWhenNull(object obj, Action action)
        {
            if (obj == null) action();
        }

        public static void ActionWhenNotNull(object obj, Action action)
        {
            if (obj != null) action();
        }

        public static void ActionWhenTrue(bool condition, Action action)
        {
            if (condition) action();
        }

        public static void ActionWhenFalse(bool condition, Action action)
        {
            if (!condition) action();
        }

        public static void ActionWhenBoolean(bool condition, Action trueAction, Action falseAction)
        {
            if (condition) trueAction();
            else falseAction();
        }

        #endregion
    }
}
