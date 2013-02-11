using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UsefulClasses;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace GameItems {
	[JsonObject(MemberSerialization.Fields)]
	public class Player : RealObject {
		[JsonProperty("money")]
		public int Money = 0;
		[JsonProperty("famous")]
		public int Famous = 0;
		[JsonProperty("people")]
		public int People = 0;
		
		[JsonProperty("position")]
		public int Position = 0;

		private int index = -1;
		[JsonProperty("index")]
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

		public Player(int playerIndex, string playerName) {
			this.Name = playerName;
			this.Index = playerIndex;
		}

		public Player(int playerIndex) {
			this.Index = playerIndex;
		}
		[JsonProperty("lose")]
		public bool Lose = false;
	}
}