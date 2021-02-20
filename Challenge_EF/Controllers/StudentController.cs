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
	public class StudentController : ControllerBase
	{
		private readonly ILogger<StudentController> _logger;

		public StudentController(ILogger<StudentController> logger)
		{
			_logger = logger;
		}
		
		/// <summary>
		/// Returns all the students
		/// </summary>
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Student>))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Get([FromServices] ChallengeDbContext dbContext)
		{
			var result = await dbContext.Students.ToListAsync();

			if (result.Count == 0)
				return BadRequest("No Students were found.");

			return Ok(result);
		}
		
		/// <summary>
		/// Returns a single student by their id
		/// </summary>
		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Student))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetFromId([FromServices] ChallengeDbContext dbContext, [FromRoute] int id)
		{
			var result = await dbContext.Students.FindAsync(id);

			if (result == null)
				return BadRequest($"The Student {id} was not found.");
			
			return Ok(result);
		}
		
		/// <summary>
		/// Creates a new student
		/// </summary>
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Student))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Create([FromServices] ChallengeDbContext dbContext, [FromBody] NameData student)
		{
			try
			{
				var result = await dbContext.Students.AddAsync(new Student
				{
					Name = student.Name,
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
		/// Updates a student
		/// </summary>
		[HttpPut("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Student))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Update([FromServices] ChallengeDbContext dbContext, [FromRoute] int id, [FromBody] NameData student)
		{
			var result = await dbContext.Students.FindAsync(id);

			if (result == null)
				return BadRequest($"The id {id} was not found.");

			result.Name = student.Name;

			try
			{
				dbContext.Students.Update(result);
				await dbContext.SaveChangesAsync();
				
				return Ok(result);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}
		
		/// <summary>
		/// Deletes a student
		/// </summary>
		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Delete([FromServices] ChallengeDbContext dbContext, [FromRoute] int id)
		{
			try
			{
				var result = new Student() { Id = id };
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