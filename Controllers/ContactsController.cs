using ContactsAPI.Data;
using ContactsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactsAPI.Controllers;


[ApiController]
[Route("api/[controller]")]
public class ContactsController : Controller
{
    private readonly ContactsApiDbContext _context;

    public ContactsController(ContactsApiDbContext context)
    {
        _context = context;
    }

    // Get lists of contacts.
    [HttpGet]
    public async Task<IActionResult> GetContactList()
    {
        return Ok(await _context.Contacts.ToListAsync());
    }
    
    // Fetch contact object detail.
    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetContactDetails([FromRoute] Guid id)
    {
        var contact = await _context.Contacts.FindAsync(id);
        if (contact != null)
        {
            return Ok(contact);   
        }
        return NotFound("Contact record not found.");
    }

    // Create contact.
    [HttpPost]
    public async Task<IActionResult> AddContact(AddContactRequest addContactRequest)
    {
        var contact = new Contact()
        {
            Id = Guid.NewGuid(),
            FullName = addContactRequest.FullName,
            Email = addContactRequest.Email,
            Address = addContactRequest.Address,
            Phone = addContactRequest.Phone
        };
        await _context.Contacts.AddAsync(contact);
        await _context.SaveChangesAsync();
        return Ok(contact);
    }
    
    // Update contact record.
    [HttpPut]
    [Route("{id:guid}")]
    public async Task<IActionResult> UpdateContact([FromRoute] Guid id, AddContactRequest updateContactRequest)
    {
        var contactObj = await _context.Contacts.FindAsync(id);
        if (contactObj != null)
        {
            contactObj.FullName = updateContactRequest.FullName;
            contactObj.Email = updateContactRequest.Email;
            contactObj.Address = updateContactRequest.Address;
            contactObj.Phone = updateContactRequest.Phone;
            _context.Contacts.Update(contactObj);
            await _context.SaveChangesAsync();
            return Ok(contactObj);
        };
        return NotFound("Contact records not found.");
    }
    
    // Partial Update contact record.
    [HttpPatch]
    [Route("{id:guid}")]
    public async Task<IActionResult> PartialUpdateContact([FromRoute] Guid id, AddContactRequest updateContactRequest)
    {
        var contactObj = await _context.Contacts.FindAsync(id);
        if (contactObj != null)
        {
            contactObj.FullName = updateContactRequest.FullName;
            contactObj.Email = updateContactRequest.Email;
            contactObj.Address = updateContactRequest.Address;
            contactObj.Phone = updateContactRequest.Phone;
            _context.Contacts.Update(contactObj);
            await _context.SaveChangesAsync();
            return Ok(contactObj);
        };
        return NotFound("Contact records not found.");
    }
    
    // Delete contact record.
    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> DeleteContact([FromRoute] Guid id)
    {
        var contact = await _context.Contacts.FindAsync(id);
        if (contact != null)
        {
            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();
            return Ok("Contact Deleted.");
        }
        return NotFound("Contact record not found.");
    }
}