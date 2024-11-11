using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Medo;

namespace Domain.Entities
{
    public sealed partial class Entities
    {
        public abstract class EntityBase
        {
            [NotMapped]
            private Guid _id = CreateUuidV7ToGuid();

            [Key]
            [Required]
            [Column("ID")]
            public Guid Id
            {
                get => _id;
                set
                {
                    if (value == Guid.Empty)
                    {
                        throw new ArgumentNullException(nameof(value), "Value is \"Guid.Empty\"");
                    }

                    _id = value;
                }
            }

            public static Guid CreateUuidV7ToGuid() => Uuid7.NewUuid7().ToGuid(matchGuidEndianness: true);
        }
    }
}