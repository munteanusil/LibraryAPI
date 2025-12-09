using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application
{
    public class ValidationMessages
    {
        public static string PropertyIsRequired(string propertyName) => $"{propertyName} is required!";

        public static string PropertyMustNotBeEmpty(string propertyName) => $"{propertyName} must  not be empty!";
        public static string PropertyHasInvalidLenght(string propertyName) => $"{propertyName} has invalid leght!";

        public static string PropertyHasInvalidValue(string propertyName) => $"{propertyName} has invalid value!";
    }
}
