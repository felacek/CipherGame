using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CipherGameData
{
    public class CipherGameContext : DbContext
    {
        public DbSet<Team> Teams { get; set; }
        public DbSet<Cipher> Ciphers { get; set; }
        public DbSet<GameState> GameStates { get; set; }

        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=CipherGame2.db");
        }
    }

    public class Team
    {
        [Key]
        [Required]
        [MinLength(3)]
        [MaxLength(3)]
        public string Code { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(20)]
        public string Name { get; set; }
    }

    public class Cipher
    {
        [Key]
        [Required]
        [MinLength(1)]
        [MaxLength(10)]
        public string Code { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(255)]
        public string Place { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(10)]
        public string Answer { get; set; }
    }

    public class GameState
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int Order { get; set; }

        [Required]
        public string TeamCode { get; set; }
        public Team Team { get; set; }
        public Cipher Cipher { get; set; }

        [Required]
        public bool IsPlaceFound { get; set; }

        [Required]
        public bool IsAnswerFound { get; set; }
    }

    public class AuditLog
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public DateTime Created { get; set; }

        [Required]

        [MinLength(1)]
        [MaxLength(255)]
        public string Log { get; set; }
    }
}
