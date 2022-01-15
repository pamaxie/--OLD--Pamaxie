using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pamaxie.Workload.Assemblies
{
    /// <summary>
    /// This stores what kind of data is being handled by the individual workload
    /// </summary>
    public class Workload
    {
        /// <summary>
        /// Constructor always requires a stream with the streams payload
        /// </summary>
        /// <param name="stream"></param>
        public Workload(Stream stream)
        {
            Payload = stream;
        }

        public Stream Payload { get; set; }
        public PayloadType PayloadType {get; set;}
        public WorkloadReturnValue WorkloadReturnValue { get; set; }
        public WorkloadRuntimeType WorkloadRuntimeType { get; set; }
    }
}
