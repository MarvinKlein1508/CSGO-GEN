using CSGO_GEN.Core.Filters;
using CSGO_GEN.Core.Models;
using Microsoft.Extensions.Primitives;
using System.Text;
using System.Web;

namespace CSGO_GEN_WebAssembly.Pages
{
    public partial class Index
    {
        private decimal _float = 0.01m;
        private int _pattern = 661;

        public WeaponFilter WeaponFilter { get; set; } = new();
        public StickerFilter StickerFilter { get; set; } = new();
        public Weapon? SelectedWeapon { get; set; }

        public List<AppliedSticker> SelectedStickers { get; set; } = new();


        public decimal Float
        {
            get => _float;
            set
            {

                decimal min_float = SelectedWeapon?.min_wear ?? 0m;
                decimal max_float = SelectedWeapon?.max_wear ?? 1m;

                if (value <= min_float)
                {
                    _float = min_float;
                }
                else if (value > max_float)
                {
                    _float = max_float;
                }
                else
                {
                    _float = value;
                }
            }
        }

        public int Pattern
        {
            get => _pattern;
            set
            {
                if (value < 0)
                {
                    _pattern = 0;
                }
                else if (value > 1000)
                {
                    _pattern = 1000;
                }
                else
                {
                    _pattern = value;
                }
            }
        }
        private void OnWeaponClicked(Weapon weapon)
        {
            SelectedWeapon = weapon;
            Float = Float;
        }

        private void OnStickerClicked(Sticker sticker)
        {
            if (SelectedStickers.Count < 5)
            {
                SelectedStickers.Add(new AppliedSticker(sticker));
            }
        }

        /// <summary>
        /// Generates a Steam Community Market URL which searchs for all skins with the selected stickers.
        /// </summary>
        /// <returns></returns>
        private string GetSteamMarketUrl()
        {
            if (!SelectedStickers.Any())
            {
                return string.Empty;
            }

            StringBuilder sb = new();
            sb.Append("https://steamcommunity.com/market/search?q=");

            string query = HttpUtility.UrlEncode($"\"{string.Join(",", SelectedStickers.Select(x => x.name))}\"");

            sb.Append(query);
            sb.Append("&descriptions=1&category_730_ItemSet%5B%5D=any&category_730_Weapon%5B%5D=any&category_730_Quality%5B%5D=#p1_price_asc");

            string url = sb.ToString();

            return url;
        }

        private string GetCsgofloatUrl()
        {
            // Example: https://csgofloat.com/db?defIndex=7&paintIndex=282&min=0.1&max=0.7&stickers=%5B%7B%22i%22:%225015%22%7D,%7B%22i%22:%225015%22%7D,%7B%22i%22:%225015%22%7D,%7B%22i%22:%225015%22%7D%5D
            StringBuilder sb = new();
            sb.Append("https://csgofloat.com/db?");

            List<string> parameters = new();

            if (SelectedWeapon is not null)
            {
                parameters.Add($"defIndex={SelectedWeapon.weapon_id}");
                parameters.Add($"paintIndex={SelectedWeapon.gen_id}");
            }

            if (SelectedStickers.Any())
            {
                // Example: [{"i":"5015"},{"i":"5015"},{"i":"5015"},{"i":"5015"}]
                var stickers = SelectedStickers.Take(4);
                List<string> sticker_values = new();
                foreach (var sticker in stickers)
                {
                    sticker_values.Add($"{{\"i\":\"{sticker.gen_id}\"}}");
                }


                parameters.Add($"stickers=[{string.Join(",", sticker_values)}]");
            }


            if (!parameters.Any())
            {
                return string.Empty;
            }

            sb.Append(string.Join("&", parameters));

            return sb.ToString();
            


        }
    }
}