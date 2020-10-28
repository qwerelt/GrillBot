﻿// <auto-generated />
using System;
using Grillbot.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Grillbot.Migrations
{
    [DbContext(typeof(GrillBotContext))]
    partial class GrillBotContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Grillbot.Database.Entity.AutoReplyItem", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("CaseSensitive")
                        .HasColumnType("bit");

                    b.Property<string>("ChannelID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CompareType")
                        .HasColumnType("int");

                    b.Property<string>("GuildID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDisabled")
                        .HasColumnType("bit");

                    b.Property<string>("MustContains")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReplyMessage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("AutoReply");
                });

            modelBuilder.Entity("Grillbot.Database.Entity.Config.GlobalConfigItem", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Key");

                    b.ToTable("GlobalConfig");
                });

            modelBuilder.Entity("Grillbot.Database.Entity.ErrorLogItem", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Data")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Errors");
                });

            modelBuilder.Entity("Grillbot.Database.Entity.Math.MathAuditLogItem", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ChannelID")
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Expression")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Result")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UnitInfo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("UserID")
                        .HasColumnType("bigint");

                    b.HasKey("ID");

                    b.HasIndex("UserID");

                    b.ToTable("MathAuditLog");
                });

            modelBuilder.Entity("Grillbot.Database.Entity.MethodConfig.MethodPerm", b =>
                {
                    b.Property<int>("PermID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<byte>("AllowType")
                        .HasColumnType("tinyint");

                    b.Property<string>("DiscordID")
                        .IsRequired()
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<int>("MethodID")
                        .HasColumnType("int");

                    b.Property<byte>("PermType")
                        .HasColumnType("tinyint");

                    b.HasKey("PermID");

                    b.HasIndex("MethodID");

                    b.ToTable("MethodPerms");
                });

            modelBuilder.Entity("Grillbot.Database.Entity.MethodConfig.MethodsConfig", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Command")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("ConfigData")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Group")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("GuildID")
                        .IsRequired()
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<bool>("OnlyAdmins")
                        .HasColumnType("bit");

                    b.Property<long>("UsedCount")
                        .HasColumnType("bigint");

                    b.HasKey("ID");

                    b.ToTable("MethodsConfig");
                });

            modelBuilder.Entity("Grillbot.Database.Entity.TeamSearch", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ChannelId")
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<string>("GuildId")
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<string>("MessageId")
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.HasKey("Id");

                    b.ToTable("TeamSearch");
                });

            modelBuilder.Entity("Grillbot.Database.Entity.Unverify.Unverify", b =>
                {
                    b.Property<long>("UserID")
                        .HasColumnType("bigint");

                    b.Property<string>("Channels")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EndDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Reason")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Roles")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("SetLogOperationID")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("StartDateTime")
                        .HasColumnType("datetime2");

                    b.HasKey("UserID");

                    b.HasIndex("SetLogOperationID")
                        .IsUnique()
                        .HasFilter("[SetLogOperationID] IS NOT NULL");

                    b.ToTable("Unverifies");
                });

            modelBuilder.Entity("Grillbot.Database.Entity.Unverify.UnverifyLog", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("FromUserID")
                        .HasColumnType("bigint");

                    b.Property<string>("JsonData")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Operation")
                        .HasColumnType("int");

                    b.Property<long>("ToUserID")
                        .HasColumnType("bigint");

                    b.HasKey("ID");

                    b.HasIndex("FromUserID");

                    b.HasIndex("ToUserID");

                    b.ToTable("UnverifyLogs");
                });

            modelBuilder.Entity("Grillbot.Database.Entity.Users.BirthdayDate", b =>
                {
                    b.Property<long>("UserID")
                        .HasColumnType("bigint");

                    b.Property<bool>("AcceptAge")
                        .HasColumnType("bit");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.HasKey("UserID");

                    b.ToTable("BirthdayDates");
                });

            modelBuilder.Entity("Grillbot.Database.Entity.Users.DiscordUser", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ApiAccessCount")
                        .HasColumnType("int");

                    b.Property<string>("ApiToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("Flags")
                        .HasColumnType("bigint");

                    b.Property<long>("GivenReactionsCount")
                        .HasColumnType("bigint");

                    b.Property<string>("GuildID")
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<long>("ObtainedReactionsCount")
                        .HasColumnType("bigint");

                    b.Property<long>("Points")
                        .HasColumnType("bigint");

                    b.Property<string>("UsedInviteCode")
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("UserID")
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<int?>("WebAdminLoginCount")
                        .HasColumnType("int");

                    b.Property<string>("WebAdminPassword")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("GuildID");

                    b.HasIndex("UsedInviteCode");

                    b.HasIndex("UserID");

                    b.ToTable("DiscordUsers");
                });

            modelBuilder.Entity("Grillbot.Database.Entity.Users.EmoteStatItem", b =>
                {
                    b.Property<string>("EmoteID")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<long>("UserID")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("FirstOccuredAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsUnicode")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastOccuredAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("UseCount")
                        .HasColumnType("bigint");

                    b.HasKey("EmoteID", "UserID");

                    b.HasIndex("UserID");

                    b.ToTable("EmoteStats");
                });

            modelBuilder.Entity("Grillbot.Database.Entity.Users.Invite", b =>
                {
                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<string>("ChannelId")
                        .IsRequired()
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long?>("CreatorId")
                        .HasColumnType("bigint");

                    b.HasKey("Code");

                    b.HasIndex("CreatorId");

                    b.ToTable("Invites");
                });

            modelBuilder.Entity("Grillbot.Database.Entity.Users.Reminder", b =>
                {
                    b.Property<long>("RemindID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("At")
                        .HasColumnType("datetime2");

                    b.Property<long?>("FromUserID")
                        .HasColumnType("bigint");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OriginalMessageID")
                        .IsRequired()
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<int>("PostponeCounter")
                        .HasColumnType("int");

                    b.Property<string>("RemindMessageID")
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<long>("UserID")
                        .HasColumnType("bigint");

                    b.HasKey("RemindID");

                    b.HasIndex("FromUserID");

                    b.HasIndex("UserID");

                    b.ToTable("Reminders");
                });

            modelBuilder.Entity("Grillbot.Database.Entity.Users.UserChannel", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ChannelID")
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<long>("Count")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("LastMessageAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("UserID")
                        .HasColumnType("bigint");

                    b.HasKey("ID");

                    b.HasIndex("UserID");

                    b.ToTable("UserChannels");
                });

            modelBuilder.Entity("Grillbot.Database.Entity.Math.MathAuditLogItem", b =>
                {
                    b.HasOne("Grillbot.Database.Entity.Users.DiscordUser", "User")
                        .WithMany("MathAudit")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Grillbot.Database.Entity.MethodConfig.MethodPerm", b =>
                {
                    b.HasOne("Grillbot.Database.Entity.MethodConfig.MethodsConfig", "Method")
                        .WithMany("Permissions")
                        .HasForeignKey("MethodID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Grillbot.Database.Entity.Unverify.Unverify", b =>
                {
                    b.HasOne("Grillbot.Database.Entity.Unverify.UnverifyLog", "SetLogOperation")
                        .WithOne("Unverify")
                        .HasForeignKey("Grillbot.Database.Entity.Unverify.Unverify", "SetLogOperationID");

                    b.HasOne("Grillbot.Database.Entity.Users.DiscordUser", "User")
                        .WithOne("Unverify")
                        .HasForeignKey("Grillbot.Database.Entity.Unverify.Unverify", "UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Grillbot.Database.Entity.Unverify.UnverifyLog", b =>
                {
                    b.HasOne("Grillbot.Database.Entity.Users.DiscordUser", "FromUser")
                        .WithMany("OutgoingUnverifyOperations")
                        .HasForeignKey("FromUserID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Grillbot.Database.Entity.Users.DiscordUser", "ToUser")
                        .WithMany("IncomingUnverifyOperations")
                        .HasForeignKey("ToUserID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("Grillbot.Database.Entity.Users.BirthdayDate", b =>
                {
                    b.HasOne("Grillbot.Database.Entity.Users.DiscordUser", "User")
                        .WithOne("Birthday")
                        .HasForeignKey("Grillbot.Database.Entity.Users.BirthdayDate", "UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Grillbot.Database.Entity.Users.DiscordUser", b =>
                {
                    b.HasOne("Grillbot.Database.Entity.Users.Invite", "UsedInvite")
                        .WithMany("UsedUsers")
                        .HasForeignKey("UsedInviteCode");
                });

            modelBuilder.Entity("Grillbot.Database.Entity.Users.EmoteStatItem", b =>
                {
                    b.HasOne("Grillbot.Database.Entity.Users.DiscordUser", "User")
                        .WithMany("UsedEmotes")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Grillbot.Database.Entity.Users.Invite", b =>
                {
                    b.HasOne("Grillbot.Database.Entity.Users.DiscordUser", "Creator")
                        .WithMany("CreatedInvites")
                        .HasForeignKey("CreatorId");
                });

            modelBuilder.Entity("Grillbot.Database.Entity.Users.Reminder", b =>
                {
                    b.HasOne("Grillbot.Database.Entity.Users.DiscordUser", "FromUser")
                        .WithMany()
                        .HasForeignKey("FromUserID");

                    b.HasOne("Grillbot.Database.Entity.Users.DiscordUser", "User")
                        .WithMany("Reminders")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Grillbot.Database.Entity.Users.UserChannel", b =>
                {
                    b.HasOne("Grillbot.Database.Entity.Users.DiscordUser", "User")
                        .WithMany("Channels")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
