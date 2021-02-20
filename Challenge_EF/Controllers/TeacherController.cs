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
	public class TeacherController : ControllerBase
	{
		private readonly ILogger<TeacherController> _logger;

		public TeacherController(ILogger<TeacherController> logger)
		{
			_logger = logger;
		}
		
		/// <summary>
		/// Returns all the teachers
		/// </summary>
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Teacher>))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Get([FromServices] ChallengeDbContext dbContext)
		{
			var result = await dbContext.Teachers.ToListAsync();

			if (result.Count == 0)
				return BadRequest("No teachers were found.");

			return Ok(result);
		}
		
		/// <summary>
		/// Returns a single teacher by their id
		/// </summary>
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
		/// Creates a new teacher
		/// </summary>
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Teacher))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Create([FromServices] ChallengeDbContext dbContext, [FromBody] NameData teacher)
		{
			try
			{
				var result = await dbContext.Teachers.AddAsync(new Teacher
				{
					Name = teacher.Name,
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
		/// Updates a teacher
		/// </summary>
		[HttpPut("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Teacher))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Update([FromServices] ChallengeDbContext dbContext, [FromRoute] int id, [FromBody] NameData teacher)
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
		/// Deletes a teacher
		/// </summary>
		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Delete([FromServices] ChallengeDbContext dbContext, [FromRoute] int id)
		{
			try
			{
				var result = new Teacher() { Id = id };
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