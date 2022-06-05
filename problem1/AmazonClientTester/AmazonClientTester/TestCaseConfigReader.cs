using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace AmazonClientTester
{
    public static class TestCaseConfigReader
    {
        /// <summary>
        /// Given a file path, open said file, deserialize it into a TestCaseConfig object, and return it
        /// </summary>
        /// <param name="filename">Filepath to open</param>
        /// <returns>TestCaseConfig deserialized from the file</returns>
        public static TestCaseConfig OpenTestCaseConfig(string filename)
        {
            var serializer = new XmlSerializer(typeof(TestCaseConfig));
            using (var stream = new StreamReader(filename))
            {
                // Ideally, we should do some error checking to validate the file
                // However, in the interest of time, I have omitted that.
                var obj = serializer.Deserialize(stream);
                return (TestCaseConfig)obj;
            }
        }
    }
}
