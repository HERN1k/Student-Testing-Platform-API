using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public sealed partial class Entities
    {
        public sealed class User : EntityBase, IEquatable<User>
        {
            [Column("Display_Name")]
            public string? DisplayName { get; set; }

            [Column("Name")]
            public string? Name { get; set; }

            [Column("Surname")]
            public string? Surname { get; set; }

            [Required]
            [Column("Mail")]
            public string? Mail { get; set; }

            [Required]
            [Column("Is_Student")]
            public bool IsStudent { get; set; }

            [Required]
            [Column("Created_Utc")]
            public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

            public bool Equals(User? other)
            {
                return other != null && Id == other.Id;
            }

            public override bool Equals(object? obj)
            {
                if (obj is User otherDto)
                {
                    return Equals(otherDto);
                }

                return false;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Id);
            }

            public override string ToString()
            {
                return string.Concat("ID: ", Id.ToString(), ", Display name: ", DisplayName ?? string.Empty);
            }
        }
    }
}