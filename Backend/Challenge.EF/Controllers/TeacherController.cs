﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Challenge.EF.Context;
using Challenge.EF.Data;
using Challenge.EF.Models;
using Challenge.EF.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Challenge.EF.Controllers
{
	[Authorize]
	[ApiController]
	[Route("[controller]")]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[Produces("application/json")]
	public class TeacherController : ControllerBase
	{
		/// <summary>
		///     Returns all the teachers, can be paged using the page and pageSize query
		/// </summary>
		/// <response code="200">Returns all the teachers</response>
		/// <response code="400">If there is no teacher on the database</response>
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Teacher>))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Get([FromServices] ChallengeDbContext dbContext, [FromQuery] int page = 0,
			[FromQuery] int pageSize = 10)
		{
			List<Teacher> result;
			if (page == 0)
				result = await dbContext.Teachers.ToListAsync();
			else
				result = await dbContext.Teachers.GetPaged(page, pageSize, teacher => teacher.Id);

			if (result.Count == 0)
				return BadRequest("No teachers were found.");

			return Ok(result);
		}

		/// <summary>
		///     Returns the number of teachers on the database
		/// </summary>
		/// <response code="200">Returns the number of teachers on the database</response>
		/// <response code="400">If there is no teacher on the database</response>
		[HttpGet("total")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetTotal([FromServices] ChallengeDbContext dbContext)
		{
			int result = await dbContext.Teachers.CountAsync();

			if (result == 0)
				return BadRequest("No teachers were found.");

			return Ok(result);
		}

		/// <summary>
		///     Returns a single teacher by their id
		/// </summary>
		/// <response code="200">Returns the teacher</response>
		/// <response code="400">If there is not teacher with this ID</response>
		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Teacher))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetFromId([FromServices] ChallengeDbContext dbContext, [FromRoute] int id)
		{
			var result = await dbContext.Teachers.FindAsync(id);

			if (result == null)
				return BadRequest($"The teacher {id} was not found.");

			return Ok(result);
		}

		/// <summary>
		///     Returns all the courses assigned to the teacher
		/// </summary>
		/// <response code="200">Returns all the courses assigned to the teacher</response>
		/// <response code="400">If there is no course associated with the teacher</response>
		[HttpGet("{id}/courses")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Course>))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetCoursesFromId([FromServices] ChallengeDbContext dbContext,
			[FromRoute] int id)
		{
			var result = await dbContext.Teaches.OrderBy(teach => teach.CourseId).Where(teach => teach.TeacherId == id)
				.Select(teach => teach.Course).ToListAsync();

			if (result == null || result.Count == 0)
				return BadRequest($"The teacher {id} was not found.");

			return Ok(result);
		}

		/// <summary>
		///     Creates a new teacher
		/// </summary>
		/// <response code="200">Returns the created teacher</response>
		/// <response code="400">If there is an error</response>
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Teacher))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Create([FromServices] ChallengeDbContext dbContext,
			[FromBody] NameData teacher)
		{
			try
			{
				var result = await dbContext.Teachers.AddAsync(new Teacher
				{
					Name = teacher.Name
				});

				await dbContext.SaveChangesAsync();

				return Ok(result.Entity);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		/// <summary>
		///     Updates a teacher
		/// </summary>
		/// <response code="200">Returns the updated teacher</response>
		/// <response code="400">If there is an error</response>
		[HttpPut("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Teacher))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Update([FromServices] ChallengeDbContext dbContext, [FromRoute] int id,
			[FromBody] NameData teacher)
		{
			var result = await dbContext.Teachers.FindAsync(id);

			if (result == null)
				return BadRequest($"The id {id} was not found.");

			result.Name = teacher.Name;

			try
			{
				dbContext.Teachers.Update(result);
				await dbContext.SaveChangesAsync();

				return Ok(result);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		/// <summary>
		///     Deletes a teacher
		/// </summary>
		/// <response code="200">If the teacher was deleted</response>
		/// <response code="400">If there is an error</response>
		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Delete([FromServices] ChallengeDbContext dbContext, [FromRoute] int id)
		{
			try
			{
				var result = new Teacher {Id = id};
				dbContext.Entry(result).State = EntityState.Deleted;
				await dbContext.SaveChangesAsync();

				return Ok();
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}
	}
}