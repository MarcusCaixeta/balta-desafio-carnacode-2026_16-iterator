// DESAFIO: Playlist - SOLUÇÃO: Padrão Iterator

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatternChallenge
{
    public class Song
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Genre { get; set; }
        public int DurationSeconds { get; set; }
        public int Year { get; set; }

        public Song(string title, string artist, string genre, int duration, int year)
        { Title = title; Artist = artist; Genre = genre; DurationSeconds = duration; Year = year; }

        public override string ToString() => $"{Title} - {Artist} ({Genre}, {Year})";
    }

    public interface ISongIterator
    {
        IEnumerator<Song> GetIterator();
    }

    public class Playlist : ISongIterator
    {
        public string Name { get; set; }
        private readonly List<Song> _songs = new();

        public Playlist(string name) => Name = name;
        public void AddSong(Song song) => _songs.Add(song);

        public IEnumerator<Song> GetIterator() => new SequentialIterator(_songs);
        public IEnumerator<Song> GetShuffleIterator() => new ShuffleIterator(_songs);
        public IEnumerator<Song> GetGenreIterator(string genre) => new GenreFilterIterator(_songs, genre);
        public IEnumerator<Song> GetOldiesIterator() => new OldiesIterator(_songs);
    }

    public class SequentialIterator : IEnumerator<Song>
    {
        private readonly List<Song> _songs;
        private int _index = -1;

        public SequentialIterator(List<Song> songs) => _songs = songs;
        public Song Current => _songs[_index];
        object IEnumerator.Current => Current;
        public bool MoveNext() => ++_index < _songs.Count;
        public void Reset() => _index = -1;
        public void Dispose() { }
    }

    public class ShuffleIterator : IEnumerator<Song>
    {
        private readonly List<Song> _shuffled;
        private int _index = -1;

        public ShuffleIterator(List<Song> songs)
        {
            _shuffled = songs.OrderBy(_ => Guid.NewGuid()).ToList();
        }

        public Song Current => _shuffled[_index];
        object IEnumerator.Current => Current;
        public bool MoveNext() => ++_index < _shuffled.Count;
        public void Reset() => _index = -1;
        public void Dispose() { }
    }

    public class GenreFilterIterator : IEnumerator<Song>
    {
        private readonly List<Song> _filtered;
        private int _index = -1;

        public GenreFilterIterator(List<Song> songs, string genre)
        {
            _filtered = songs.Where(s => s.Genre == genre).ToList();
        }

        public Song Current => _filtered[_index];
        object IEnumerator.Current => Current;
        public bool MoveNext() => ++_index < _filtered.Count;
        public void Reset() => _index = -1;
        public void Dispose() { }
    }

    public class OldiesIterator : IEnumerator<Song>
    {
        private readonly List<Song> _oldies;
        private int _index = -1;

        public OldiesIterator(List<Song> songs)
        {
            _oldies = songs.Where(s => s.Year < 2000).OrderBy(s => s.Year).ToList();
        }

        public Song Current => _oldies[_index];
        object IEnumerator.Current => Current;
        public bool MoveNext() => ++_index < _oldies.Count;
        public void Reset() => _index = -1;
        public void Dispose() { }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Playlist (Iterator Pattern) ===\n");

            var playlist = new Playlist("Favoritas");
            playlist.AddSong(new Song("Bohemian Rhapsody", "Queen", "Rock", 354, 1975));
            playlist.AddSong(new Song("Imagine", "John Lennon", "Pop", 183, 1971));
            playlist.AddSong(new Song("Smells Like Teen Spirit", "Nirvana", "Rock", 301, 1991));
            playlist.AddSong(new Song("Hotel California", "Eagles", "Rock", 391, 1976));

            void Play(IEnumerator<Song> it)
            {
                int i = 1;
                while (it.MoveNext())
                    Console.WriteLine($"{i++}. {it.Current}");
            }

            Console.WriteLine("Sequencial:"); Play(playlist.GetIterator());
            Console.WriteLine("\nAleatório:"); Play(playlist.GetShuffleIterator());
            Console.WriteLine("\nGênero Rock:"); Play(playlist.GetGenreIterator("Rock"));

        }
    }
}
