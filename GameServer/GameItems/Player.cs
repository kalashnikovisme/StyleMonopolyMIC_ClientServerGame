using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UsefulClasses;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace GameItems {
	[DataContract]
	public class Player : RealObject {
		[DataMember]
		public int Money = 0;
		[DataMember]
		public int Famous = 0;
		[DataMember]
		public int People = 0;
		
	
		private int position = 0;
		[DataMember]
		public int Position {
			get {
				return position;
			}
			set {
				position = value;
			}
		}

		private int index = -1;
		[DataMember]
		public int Index {
			get {
				return index;
			}
			set {
				if (value < 0) {
					throw new Exception("Индекс игрока не может быть меньше нуля. Игрок - " + this.Name);
				}
			}
		}

		public Player(string playerName) {
			this.Name = playerName;
		}

		public Player(int playerIndex) {
			this.Index = playerIndex;
		}

		public bool Lose = false;
	}
}