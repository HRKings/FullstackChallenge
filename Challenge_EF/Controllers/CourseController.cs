﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Challenge_EF.Context;
using Challenge_EF.Data;
using Challenge_EF.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Challenge_EF.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class CourseController : ControllerBase
	{
		private readonly ILogger<CourseController> _logger;

		public CourseController(ILogger<CourseController> logger)
		{
			_logger = logger;
		}

		/// <summary>
		///     Returns all the courses
		/// </summary>
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Course>))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Get([FromServices] ChallengeDbContext dbContext)
		{
			var result = await dbContext.Courses.ToListAsync();

			if (result.Count == 0)
				return BadRequest("No Courses were found.");

			return Ok(result);
		}

		/// <summary>
		///     Returns a single course by their id
		/// </summary>
		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Course))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetFromId([FromServices] ChallengeDbContext dbContext, [FromRoute] int id)
		{
			var result = await dbContext.Courses.FindAsync(id);

			if (result == null)
				return BadRequest($"The Course {id} was not found.");

			return Ok(result);
		}

		/// <summary>
		///     Creates a new course
		/// </summary>
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Course))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Create([FromServices] ChallengeDbContext dbContext, [FromBody] NameData course)
		{
			try
			{
				var result = await dbContext.Courses.AddAsync(new Course
				{
					Name = course.Name
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
		///     Updates a course
		/// </summary>
		[HttpPut("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Course))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Update([FromServices] ChallengeDbContext dbContext, [FromRoute] int id,
			[FromBody] NameData course)
		{
			var result = await dbContext.Courses.FindAsync(id);

			if (result == null)
				return BadRequest($"The id {id} was not found.");

			result.Name = course.Name;

			try
			{
				dbContext.Courses.Update(result);
				await dbContext.SaveChangesAsync();

				return Ok(result);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		/// <summary>
		///     Deletes a course
		/// </summary>
		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Delete([FromServices] ChallengeDbContext dbContext, [FromRoute] int id)
		{
			try
			{
				var result = new Course {Id = id};
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