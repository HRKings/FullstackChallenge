﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Challenge_Dapper.Models;
using Challenge_Dapper.Repositories;
using Challenge_EF.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Challenge_Dapper.Controllers
{
	[Authorize]
	[ApiController]
	[Route("[controller]")]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[Produces("application/json")]
	public class StudentController : ControllerBase
	{
		/// <summary>
		///     Returns all the students, can be paged using the page and pageSize query
		/// </summary>
		/// <response code="200">Returns all students</response>
		/// <response code="400">If there is no students in the database</response>
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Student>))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Get([FromQuery] int page = 0, [FromQuery] int pageSize = 10)
		{
			List<Student> result;
			if (page == 0)
				result = (List<Student>) await StudentRepository.GetAll();
			else
				result = (List<Student>) await StudentRepository.GetPaged(page, pageSize);

			if (result.Count == 0)
				return BadRequest("No Students were found.");

			return Ok(result);
		}
		
		/// <summary>
		///     Returns the number of students on the database
		/// </summary>
		/// <response code="200">Returns the number of students on the database</response>
		/// <response code="400">If there is no students in the database</response>
		[HttpGet("total")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Student>))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetTotal()
		{
			int result = await StudentRepository.Count();
			
			if (result == 0)
				return BadRequest("No Students were found.");

			return Ok(result);
		}

		/// <summary>
		///     Returns a single student by their id
		/// </summary>
		/// <response code="200">Returns the student</response>
		/// <response code="400">If there is no student with this ID</response>
		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Student))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetFromId([FromRoute] int id)
		{
			var result = await StudentRepository.Get(id);

			if (result == null)
				return BadRequest($"The Student {id} was not found.");

			return Ok(result);
		}
		
		/// <summary>
		///     Returns all the courses assigned to the student
		/// </summary>
		/// <response code="200">Returns all the courses assigned to the student</response>
		/// <response code="400">If there is no course associated with the student</response>
		[HttpGet("{id}/courses")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Course>))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetCoursesFromId([FromRoute] int id)
		{
			var result = await StudentRepository.Courses(id);

			if (result == null || !result.Any())
				return BadRequest($"The student {id} was not found.");

			return Ok(result);
		}

		/// <summary>
		///     Creates a new student
		/// </summary>
		/// <response code="200">Returns the created student</response>
		/// <response code="400">If there is an error</response>
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Student))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Create([FromBody] NameData student)
		{
			try
			{
				var result = await StudentRepository.Insert(student.Name);
				return Ok(result);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		/// <summary>
		///     Updates a student
		/// </summary>
		/// <response code="200">Returns the updated student</response>
		/// <response code="400">If there is an error</response>
		[HttpPut("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Student))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Update([FromRoute] int id,
			[FromBody] NameData student)
		{
			var result = await StudentRepository.Get(id);

			if (result == null)
				return BadRequest($"The id {id} was not found.");

			result.Name = student.Name;

			try
			{
				await StudentRepository.Update(result);
				return Ok(result);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		/// <summary>
		///     Deletes a student
		/// </summary>
		/// <response code="200">If the student was deleted</response>
		/// <response code="400">If there is an error</response>
		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> Delete([FromRoute] int id)
		{
			try
			{
				await StudentRepository.Delete(id);
				return Ok();
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}
	}
}