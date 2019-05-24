namespace steamLauncher.Logic.Helpers
{
    using System;
    using System.IO;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    /// <summary>
    /// The xml serialization helper.
    /// </summary>
    public static class XmlSerializationHelper
    {
        /// <summary>
        /// The factory.
        /// </summary>
        private static XmlSerializerFactory factory = new XmlSerializerFactory();

        /// <summary>
        /// The deserialize.
        /// </summary>
        /// <param name="filePath">
        /// The file path.
        /// </param>
        /// <typeparam name="TObject">
        /// Type of object to deserialize.
        /// </typeparam>
        /// <returns>
        /// The <see cref="TObject"/>.
        /// The deserialized object.
        /// </returns>
        /// <exception cref="FileNotFoundException">
        /// Provided file is not found.
        /// </exception>
        /// <exception cref="ApplicationException">
        /// Xml structure is corrupted or unsupported.
        /// </exception>
        public static TObject Deserialize<TObject>(string filePath)
            where TObject : class
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Xml file not found: " + filePath);
            }

            var serializer = factory.CreateSerializer(typeof(TObject));
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (var reader = new XmlTextReader(stream) { })
                {
                    if (!serializer.CanDeserialize(reader))
                    {
                        throw new ApplicationException("The proved XML file is corrupted or is of unsupported format.");
                    }

                    return serializer.Deserialize(reader) as TObject;
                }
            }
        }

        /// <summary>
        /// The serialize.
        /// </summary>
        /// <param name="filePath">
        /// The file path.
        /// </param>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <typeparam name="TObject">
        /// Type of object to serialize.
        /// </typeparam>
        public static void Serialize<TObject>(string filePath, TObject item)
            where TObject : class
        {
            var serializer = factory.CreateSerializer(typeof(TObject));
            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (var writer = new XmlTextWriter(stream, Encoding.UTF8) { Formatting = Formatting.Indented })
                {
                    serializer.Serialize(writer, item);
                }
            }
        }
    }
}