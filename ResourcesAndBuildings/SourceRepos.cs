using System;
using System.Collections;
using System.Collections.Generic;

namespace LabOOP1
{
    public class Repos
    {

        public static T GetSourceOfType<T>() where T : Source, new()
        {
            var newProduct = new T();
            var currentType = typeof(T);
            return newProduct;
        }
    }
}