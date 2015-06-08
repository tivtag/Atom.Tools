// <copyright file="SpriteDatabaseSaver.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tools.SpriteTool.Data.SpriteDatabaseSaver class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Tools.SpriteTool.Database
{
    using System;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Linq;
    using Atom.Diagnostics;
    using Atom.Storage;
    using Atom.Xna;
    using System.Globalization;

    /// <summary>
    /// Implements a mechanism that saves a SpriteDatabase.
    /// </summary>
    public sealed class SpriteDatabaseSaver
    {
        /// <summary>
        /// Initializes a new instance of the SpriteDatabaseSaver class.
        /// </summary>
        /// <param name="log">
        /// Represents the ILog to which error information is written.
        /// </param>
        /// <param name="textureLoader">
        /// Provides a mechanism that allows loading of Texture2D assets.
        /// </param>
        public SpriteDatabaseSaver( ILog log, ITexture2DLoader textureLoader )
        {
            Contract.Requires<ArgumentNullException>( log != null );

            this.log = log;
            this.databaseReaderWriter = new SpriteDatabase.ReaderWriter( textureLoader );
        }

        /// <summary>
        /// Saves the given SpriteDatabase at the given location.
        /// </summary>
        /// <param name="database">
        /// The database that is supposed to be saved.
        /// </param>
        /// <param name="outputPath">
        /// The path the database should be saved at.
        /// </param>
        /// <param name="shouldPreserveSpriteProperties">
        /// Gets or sets a value indicating whether sprite properties,
        /// such as Color, Scale, etc. are preserved from previously existing Sprite.
        /// </param>
        public void Save( SpriteDatabase database, string outputPath, string shouldPreserveSpriteProperties )
        {
            SpriteDatabase originalDatabase = this.LoadOriginalDatabase( outputPath );
            bool successful = true;

            if( "true".Equals( shouldPreserveSpriteProperties, StringComparison.OrdinalIgnoreCase ) )
            {
                this.PreservSpriteProperties( database, originalDatabase );

                if( originalDatabase != null )
                {
                    successful = this.InsertAnimatedSprites( database, originalDatabase );
                }
            }
            else if( "all".Equals( shouldPreserveSpriteProperties, StringComparison.OrdinalIgnoreCase ) )
            {
                successful = this.PreservAllSpriteProperties( database, outputPath );
            }

            if( !successful )
            {
                this.BackupOriginal( originalDatabase, outputPath );
            }
            
            this.Save( database, outputPath );
        }

        private bool PreservAllSpriteProperties( SpriteDatabase database, string outputDatabasePath )
        {
            bool successful = true;
            string outputDirectory = Path.GetDirectoryName( outputDatabasePath );

            log.WriteLine();
            foreach( string databaseFile in Directory.GetFiles( outputDirectory, "*" + SpriteDatabase.ReaderWriter.Extension ) )
            {
                log.WriteLine( databaseFile + ":" );

                var originalDatabase = LoadOriginalDatabase( databaseFile );
                this.PreservSpriteProperties( database,originalDatabase );

                if( !this.InsertAnimatedSprites( database, originalDatabase ) )
                {
                    successful = false;
                }
            }
            log.WriteLine();

            return successful;
        }

        /// <summary>
        /// Perserves the properties of the sprites of the given SpriteDatabase
        /// by attempting to load the SpriteDatabase at the given
        /// </summary>
        /// <param name="database">
        /// The database that is supposed to be saved.
        /// </param>
        /// <param name="originalDatabase">
        /// The database the new database is build upon originally.
        /// </param>
        private void PreservSpriteProperties( SpriteDatabase database, SpriteDatabase originalDatabase)
        {
            if( originalDatabase != null )
            {
                int count = 0;

                foreach( var originalSprite in originalDatabase.Atlas.Sprites )
                {
                    var sprite = database.FindSprite( originalSprite.Name );

                    if( sprite != null )
                    {
                        sprite.Color = originalSprite.Color;
                        sprite.Scale = originalSprite.Scale;
                        sprite.Origin = originalSprite.Origin;
                        sprite.Effects = originalSprite.Effects;
                        sprite.Rotation = originalSprite.Rotation;

                        ++count;
                    }
                }

                if( count > 0 )
                {
                    this.log.WriteLine(
                        LogSeverities.Info,
                        "{0} sprites were preserved.",
                        count.ToString( CultureInfo.CurrentCulture )
                    );
                }
            }
        }

        /// <summary>
        /// Loads and returns the SpriteDatabase that already exists at the given path.
        /// </summary>
        /// <param name="path">
        /// The path of the database to load.
        /// </param>
        /// <returns>
        /// The SpriteDatabase that has been loaden;
        /// or null if no SpriteDatabase exists at the given location.
        /// </returns>
        private SpriteDatabase LoadOriginalDatabase( string path )
        {
            if( File.Exists( path ) )
            {
                using( var stream = File.OpenRead( path ) )
                {
                    var originalDatabase = new SpriteDatabase();
                    this.databaseReaderWriter.Deserialize(
                        originalDatabase,
                        new BinaryDeserializationContext( stream )
                    );

                    return originalDatabase;
                }
            }

            return null;
        }

        /// <summary>
        /// Loads the AnimatedSprites
        /// </summary>
        /// <param name="database">
        /// The database that is supposed to be saved.
        /// </param>
        /// <param name="originalDatabase">
        /// The database the new database is build upon originally.
        /// </param>
        /// <returns>
        /// true if all AnimatedSprites could be inserted;
        /// otherwise false.
        /// </returns>
        private bool InsertAnimatedSprites( SpriteDatabase database, SpriteDatabase originalDatabase )
        {
            bool everythingWasInserted = true;
            int insertCount = 0;

            foreach( var animatedSprite in originalDatabase.AnimatedSprites )
            {
                if( this.VerifyAnimatedSprite( database, animatedSprite ) )
                {
                    database.AnimatedSprites.Add( animatedSprite );
                    ++insertCount;
                }
                else
                {
                    this.log.WriteLine( 
                        LogSeverities.Warning,
                        "AnimatedSprite '{0}' could not be inserted into the SpriteDatabase.", animatedSprite.Name
                    );

                    this.log.WriteLine();
                    everythingWasInserted = false;
                }
            }

            if( insertCount > 0 )
            {
                this.log.WriteLine(
                    LogSeverities.Info,
                    "{0} animated sprites were preserved.", 
                    insertCount.ToString( CultureInfo.CurrentCulture )
                );
            }

            return everythingWasInserted;
        }

        /// <summary>
        /// Verifies that the specified AnimatedSprite could be inserted into the specified SpriteDatabase.
        /// </summary>
        /// <param name="database">
        /// The insertion target of the animatedSprite.
        /// </param>
        /// <param name="animatedSprite">
        /// The AnimatedSprite to verify.
        /// </param>
        /// <returns>
        /// true if the AnimatedSprite could be inserted into the SpriteDatabase;
        /// otherwise false.
        /// </returns>
        private bool VerifyAnimatedSprite( SpriteDatabase database, AnimatedSprite animatedSprite )
        {
            bool result = true;

            for( int index = 0; index < animatedSprite.FrameCount; ++index )
            {
                Sprite frameSprite = animatedSprite[index].Sprite;

                if( frameSprite != null )
                {
                    if( !database.Sprites.Any( sprite => sprite.Name.Equals( frameSprite.Name, StringComparison.OrdinalIgnoreCase ) ) )
                    {
                        if( result )
                        {
                            this.log.WriteLine( "------------" );
                        }

                        result = false;
                        this.log.WriteLine( LogSeverities.Warning, "Sprite '{0}' is missing..", frameSprite.Name );
                    }
                }
            }

            if( !result )
            {
                this.log.WriteLine( "------------" );
            }

            return result;
        }

        /// <summary>
        /// Actually saves the specified SpriteDatabase to the given outputPath.
        /// </summary>
        /// <param name="database">
        /// The SpriteDatabase to save.
        /// </param>
        /// <param name="outputPath">
        /// The path at which the SpriteDatabase should be saved.
        /// </param>
        private void Save( SpriteDatabase database, string outputPath )
        {
            string directory = Path.GetDirectoryName( outputPath );
            Directory.CreateDirectory( directory );

            using( var stream = File.Open( outputPath, FileMode.Create, FileAccess.Write ) )
            {
                this.databaseReaderWriter.Serialize(
                    database,
                    new BinarySerializationContext( stream )
                );
            }
        }

        /// <summary>
        /// Backsup the specified original SpriteDatabase.
        /// </summary>
        /// <param name="originalDatabase">
        /// The database the new database is build upon originally.
        /// </param>
        /// <param name="outputPath">
        /// The path at which the new SpriteDatabase should be saved.
        /// </param>
        private void BackupOriginal( SpriteDatabase originalDatabase, string outputPath )
        {
            string fullPath = GetBackupFilePath( originalDatabase, outputPath ) ;

            this.Save( originalDatabase, fullPath );

            this.log.WriteLine();
            this.log.WriteLine( LogSeverities.Warning, "The original SpriteDatabase has been backed up at {0}.", fullPath );
            this.log.WriteLine();
        }

        /// <summary>
        /// Gets the full file path at which the backup of the specified original SpriteDatabase should be created.
        /// </summary>
        /// <param name="originalDatabase">
        /// The database the new database is build upon originally.
        /// </param>
        /// <param name="outputPath">
        /// The path at which the new SpriteDatabase should be saved.
        /// </param>
        /// <returns>
        /// The full file path at which the backup should be saved.
        /// </returns>
        private static string GetBackupFilePath( SpriteDatabase originalDatabase, string outputPath )
        {
            string basePath = Path.Combine( Path.GetDirectoryName( outputPath ), "Backup" );
            string fileName = string.Format(
                CultureInfo.InvariantCulture,
                "{0}_Backup_{1}{2}",
                Path.GetFileNameWithoutExtension( originalDatabase.Name ),
                DateTime.Now.ToFileTime().ToString( CultureInfo.InvariantCulture ),
                SpriteDatabase.ReaderWriter.Extension
            );

            return Path.Combine( basePath, fileName );
        }

        /// <summary>
        /// The IObjectReaderWriter that is used to de-/serialize the SpriteDatabases.
        /// </summary>
        private readonly IObjectReaderWriter<SpriteDatabase> databaseReaderWriter;

        /// <summary>
        /// Represents the ILog to which error information is written.
        /// </summary>
        private readonly ILog log;
    }
}
