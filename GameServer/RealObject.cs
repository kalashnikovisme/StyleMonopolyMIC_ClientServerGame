using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UsefulClasses {
	[DataContract]
	abstract public class RealObject {
		private string name = "";
		[DataMember]
		public string Name {
			get {
				return name;
			}
			set {
				name = value;
			}
		}

		private string info = "";
		[DataMember]
		public string Info {
			get {
				return info;
			}
			set {
				info = value;
			}
		}	
	}
}