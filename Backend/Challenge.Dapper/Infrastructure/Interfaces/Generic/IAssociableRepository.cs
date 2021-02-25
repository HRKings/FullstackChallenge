﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;

namespace Infrastructure.Interfaces.Generic
{
	public interface IAssociableRepository<T> : ICrudRepository<T>
	{
		public Task<List<Course>> GetCoursesFromId(int id);
	}
}