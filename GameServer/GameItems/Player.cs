﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UsefulClasses;

namespace GameItems {
	public class Player : RealObject {
		public int Money = 0;
		public int Famous = 0;
		public int People = 0;
		
		private const int NOT_IN_FIELD = -1;
		
		private int position = NOT_IN_FIELD;
		public int Position {
			get {
				if (position == NOT_IN_FIELD) {
					throw new Exception("Player did not make a single move");
				}
				return position;
			}
			set {
				position = value;
			}
		}

		public Player(string playerName) {
			this.Name = playerName;
		}

		public Player(int playerIndex)

		public bool Lose = false;
	}
}