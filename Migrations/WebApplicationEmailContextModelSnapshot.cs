﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApplicationEmail.Data;

#nullable disable

namespace WebApplicationEmail.Migrations
{
    [DbContext(typeof(WebApplicationEmailContext))]
    partial class WebApplicationEmailContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WebApplicationEmail.Models.Correo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Asunto")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Contenido")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PersonaId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PersonaId")
                        .IsUnique();

                    b.ToTable("Correo");
                });

            modelBuilder.Entity("WebApplicationEmail.Models.CorreoPersona", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CorreoId")
                        .HasColumnType("int");

                    b.Property<bool>("Leido")
                        .HasColumnType("bit");

                    b.Property<int>("PersonaId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CorreoId");

                    b.HasIndex("PersonaId");

                    b.ToTable("CorreoPersona");
                });

            modelBuilder.Entity("WebApplicationEmail.Models.Persona", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nombre")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Persona");
                });

            modelBuilder.Entity("WebApplicationEmail.Models.Correo", b =>
                {
                    b.HasOne("WebApplicationEmail.Models.Persona", "Remitente")
                        .WithOne("Correo")
                        .HasForeignKey("WebApplicationEmail.Models.Correo", "PersonaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Remitente");
                });

            modelBuilder.Entity("WebApplicationEmail.Models.CorreoPersona", b =>
                {
                    b.HasOne("WebApplicationEmail.Models.Correo", null)
                        .WithMany("CorreoPersonas")
                        .HasForeignKey("CorreoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApplicationEmail.Models.Persona", null)
                        .WithMany("CorreoPersonas")
                        .HasForeignKey("PersonaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WebApplicationEmail.Models.Correo", b =>
                {
                    b.Navigation("CorreoPersonas");
                });

            modelBuilder.Entity("WebApplicationEmail.Models.Persona", b =>
                {
                    b.Navigation("Correo");

                    b.Navigation("CorreoPersonas");
                });
#pragma warning restore 612, 618
        }
    }
}
