﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ShowApp.Entity
{
	public class Show
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Stars { get; set; }
		public string Description { get; set; }
		public string Image { get; set; }
		public int Year { get; set; }
		public int CategoryId { get; set; }
		public Category Category { get; set; }
		public int GenreId { get; set; }
		public Genre Genre { get; set; }
	}
}
