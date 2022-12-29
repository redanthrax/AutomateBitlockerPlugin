using AutomateBitlockerPlugin.Domain.Constants;
using AutomateBitlockerPlugin.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AutomateBitlockerPlugin.Application.Common {
    public static class ObjConvert {
        /// <summary>
        /// Create a list of serialized objects.
        /// </summary>
        /// <param name="objects">List of objects</param>
        /// <returns>Collection of serialized objects with keys as the type</returns>
        public static NameValueCollection ToCollection(List<Object> objects) {
            var collection = new NameValueCollection();
            foreach (object item in objects) {

                var key = item.GetType().Name;
                var data = WebUtility.UrlEncode(SerializeObject(item));

                collection.Add(key, data);
            }

            return collection;
        }

        /// <summary>
        /// Returns a list of objects from a collection of serialized objects.
        /// </summary>
        /// <param name="collection">Collection of objects</param>
        /// <returns>List of objects</returns>
        public static List<object> FromCollection(NameValueCollection collection) {
            var objects = new List<object>();

            objects.Add(
                DeserializeObject<BitlockerTPM>(WebUtility.UrlDecode(collection["BitlockerTPM"]))
                );

            return objects;
        }

        /// <summary>
        /// Helper function to serialize object into XML string.
        /// </summary>
        /// <param name="toSerialize">Object to serialize</param>
        /// <returns>XML string of object serialized</returns>
        public static string SerializeObject<T>(this T toSerialize) {
            var xmlSerializer = new XmlSerializer(toSerialize.GetType());

            using (var textWriter = new StringWriter()) {
                xmlSerializer.Serialize(textWriter, toSerialize);
                return textWriter.ToString();
            }
        }

        /// <summary>
        /// Helper function to deserialized an XML string into an object.
        /// </summary>
        /// <typeparam name="T">Type of object to be deserialized</typeparam>
        /// <param name="xmlData">Xml string of the object</param>
        /// <returns>Deserialized object</returns>
        public static object DeserializeObject<T>(string xmlData) {
            var serializer = new XmlSerializer(typeof(T));
            var stringReader = new StringReader(xmlData);

            T returnObject = (T)serializer.Deserialize(stringReader);
            return returnObject;
        }
    }
}
