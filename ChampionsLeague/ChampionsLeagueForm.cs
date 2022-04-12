using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChampionsLeague
{
    public partial class ChampionsLeagueForm : Form
    {
        PlayersRecords records = new PlayersRecords();
        DataGridViewRow? selectedRow = null;
        int index = 0;
        public ChampionsLeagueForm()
        {
            InitializeComponent();

            records.PlayersCountChanged += PlayersCountChangedHandler;
        }

        private void PlayersCountChangedHandler(object sender, PlayersCountChangedEventArgs e)
        {
            DateTime time = DateTime.Now;
            listBoxEvents.Items.Add(time.ToString("T") + " | Změna počtu hráčů z " + e.OldCount + " na " + e.NewCount);

            //dataGridViewPlayers.DataSource = records.players;
            List<Player> list = new List<Player>();

            for (int i = 0; i < records.Count; i++)
            {
                list.Add((Player)records.players[i]);
            }


            dataGridViewPlayers.DataSource = list;
        }

        private void btnAddPlayer_Click(object sender, EventArgs e)
        {
            AddPlayerForm addPlayer = new AddPlayerForm();
            
            if (addPlayer.ShowDialog() == DialogResult.OK)
            {
                records.Add(addPlayer.Player);
            }

        }

        private void btnModifyPlayer_Click(object sender, EventArgs e)
        {
            if (selectedRow != null)
            {
                AddPlayerForm addPlayer = new AddPlayerForm();
                FootballClub fbKlub = FootballClub.None;
                switch (selectedRow.Cells[1].Value.ToString())
                {
                    case "FCPorto":
                        fbKlub = FootballClub.FCPorto;
                        break;
                    case "Arsenal":
                        fbKlub = FootballClub.Arsenal;
                        break;
                    case "RealMadrid":
                        fbKlub = FootballClub.RealMadrid;
                        break;
                    case "Chelsea":
                        fbKlub = FootballClub.Chelsea;
                        break;
                    case "Barcelona":
                        fbKlub = FootballClub.Barcelona;
                        break;
                }
                addPlayer.Player = new Player(selectedRow.Cells[0].Value.ToString(), fbKlub, int.Parse(selectedRow.Cells[2].Value.ToString()));
                if (addPlayer.ShowDialog() == DialogResult.OK)
                {
                    records.Delete(index);
                    records.Add(addPlayer.Player);
                }
            }
            
        }

        private void dataGridViewPlayers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = dataGridViewPlayers.Rows[e.RowIndex];
            index = e.RowIndex;
        }

        private void btnRemovePlayer_Click(object sender, EventArgs e)
        {
            records.Delete(index);
        }

        private void btnBestClubs_Click(object sender, EventArgs e)
        {
            FootballClub[] clubs;
            int goals = 0;
            if (records.Count > 0)
            {
                records.FindBestClubs(out clubs, out goals);
                new TopClubs(clubs, goals).ShowDialog();
            }
        }

        private void btnUlozit_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Player Files | *.players";
            string cestaKSouboru;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                cestaKSouboru = saveFileDialog.FileName;
                PlayersFileSerializerDeserializer file = new PlayersFileSerializerDeserializer(records, cestaKSouboru);
                file.Save();
            }

        }

        private void btnNacist_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Player Files | *.players";
            string cestaKSouboru;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                cestaKSouboru = openFileDialog.FileName;
                PlayersFileSerializerDeserializer file = new PlayersFileSerializerDeserializer(records, cestaKSouboru);
                file.Load();

                List<Player> list = new List<Player>();

                for (int i = 0; i < records.Count; i++)
                {
                    list.Add((Player)records.players[i]);
                }


                dataGridViewPlayers.DataSource = list;
            }
        }
    }
}
