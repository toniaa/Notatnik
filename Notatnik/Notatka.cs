﻿using System;
using System.Collections;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace Notatnik
{
    public class Notatka : INotatka
    {
        public FlowDocument Tekst { get; set; }

        public string Tytul { get; set; }
        public string Autor { get; set; }
        public Kategoria Kategoria { get; set; }

        public DateTime DataUtworzenia { get; set; }
        public DateTime DataModyfikacji { get; set; }
        public bool Wyroznienie { get; set; }

        public HistoriaEdycji HistoriaEdycji { get; set; }

        /// <summary>
        /// Tworzenie głównej instancji pustej notatki.
        /// </summary>
        /// <param name="kategorie">
        /// Obiekt, z którego notatka ma pobrać pulę dostępnych kategorii.
        /// </param>
        public Notatka(Kategorie kategorie)
        {
            Wyroznienie = false;
            DataUtworzenia = DateTime.Now;
            DataModyfikacji = DateTime.Now;
            Kategoria = kategorie.GetKategoria(0);
            Tekst = new FlowDocument();
            Tekst.FontFamily = SystemFonts.MessageFontFamily;
            Tekst.FontSize = 12;
            Tekst.Foreground = Brushes.Black;
            HistoriaEdycji = new HistoriaEdycji();
        }

        public void ZapiszStanDoHistorii()
        {
            HistoriaEdycji.DodajWpis(new WpisHistorii(this));
        }

        public void WczytajStanZHistorii(WpisHistorii wpis)
        {
            PrzepiszTekst(wpis.Tekst, this.Tekst);
            this.Kategoria = wpis.Kategoria;
            this.DataModyfikacji = wpis.DataModyfikacji;
            this.Tytul = wpis.Tytul;
            this.Autor = wpis.Autor;
        }

        /// <summary>
        /// Tworzenie kopii głębokiej dokumentu typu FlowDocument - skopiowanie jego treści.
        /// </summary>
        /// <param name="from">
        /// Dokument, z którego ma być pobrana treść.
        /// </param>
        /// <param name="to">
        /// Dokument, do którego ma być zapisana treść.
        /// </param>
        public static void PrzepiszTekst(FlowDocument from, FlowDocument to)
        {
            TextRange range1, range2;
            MemoryStream stream = new MemoryStream();
            range1 = new TextRange(from.ContentStart, from.ContentEnd);
            range2 = new TextRange(to.ContentStart, to.ContentEnd);
            range1.Save(stream, DataFormats.XamlPackage);
            range2.Load(stream, DataFormats.XamlPackage);
        }

        /// <summary>
        /// Tekstowa reprezentacja obiektu.
        /// </summary>
        /// <returns>
        /// Tytuł notatki.
        /// </returns>
        public override string ToString()
        {
            if (Tytul == null) return "";
            else return Tytul;
        }
    }
}
