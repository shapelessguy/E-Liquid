using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace E_Liquid
{
    public partial class Form1 : Form
    {
        Kernel kernel;
        static public int id = 0;
        public Form1()
        {
            InitializeComponent();
            FormClosing += (o, e) => { kernel.active = false; };
            Thread kernelT = new Thread(kernelStart);
            kernelT.Start();

        }


        private void kernelStart() { kernel = new Kernel(); kernel.activity(); }

        private void Form1_Load(object sender, EventArgs e)
        {
            /*
            recipe recipe1 = new recipe(5f, 0.50f, 0.50f, 0f, 4f, null);
            recipe1.aroma["aroma1"] = 0.01f; recipe1.aroma["aroma2"] = 0.01f;

            recipe recipe2 = new recipe(5f, 0.50f, 0.50f, 0f, 4f, null);
            recipe2.aroma["aroma1"] = 0.01f; recipe2.aroma["aroma3"] = 0.01f;


            ingredients result = AddRecipes(new List<recipe> { recipe1, recipe2});
            result.print("ml", true);

            recipe final = new recipe(10f, 0.50f, 0.50f, 0f, 4f, null);
            final.aroma["aroma1"] = 0.01f; final.aroma["aroma2"] = 0.01f;
            ingredients mix = Mix(final, result);

            float min = FindMin(final, result);

            mix.print("ml", true);
            Console.WriteLine("Minimum quantity for this mix: " + min);
            */
        }

        private ingredients Mix(recipe rec, ingredients input=null)
        {
            var ing = new List<object> { "mix", "ml", input, rec.total, rec.vg_conc, rec.pg_conc,
                                                rec.wat_conc, rec.nic, rec.aroma };
            kernel.buffer[id] = ing;
            while (!kernel.buffer_out.Keys.Contains(id)) { Thread.Sleep(50); };
            ingredients output = (ingredients)kernel.buffer_out[id][0];
            id += 1;
            return output;
        }
        private float FindMin(recipe rec, ingredients input=null)
        {
            var ing = new List<object> { "min", "ml", input, 0, rec.vg_conc, rec.pg_conc,
                                                rec.wat_conc, rec.nic, rec.aroma };
            kernel.buffer[id] = ing;
            while (!kernel.buffer_out.Keys.Contains(id)) { Thread.Sleep(50); };
            float output = (float)kernel.buffer_out[id][0];
            id += 1;
            return output;
        }

        private ingredients AddRecipes(List<recipe> recipes)
        {
            if (recipes.Count == 0) return new ingredients();
            List<ingredients> ingr = new List<ingredients>();
            foreach(recipe rec in recipes)
            {
                var ing = new List<object> { "mix", "ml", null, rec.total, rec.vg_conc, rec.pg_conc, 
                                                rec.wat_conc, rec.nic, rec.aroma };
                kernel.buffer[id] = ing;
                while (!kernel.buffer_out.Keys.Contains(id)) { Thread.Sleep(50); };
                ingr.Add((ingredients)kernel.buffer_out[id][0]);
                id += 1;
            }
            ingredients sum = new ingredients();
            foreach(ingredients ing in ingr)
            {
                var input = new List<object> { "add", "ml", sum, ing };
                kernel.buffer[id] = input;
                while (!kernel.buffer_out.Keys.Contains(id)) { Thread.Sleep(50); };
                sum = (ingredients)kernel.buffer_out[id][0];
                id += 1;
            }
            return sum;
        }

        private float cleanedText(string text)
        {
            float res = 0;
            try { res = (float)Convert.ToDouble(text.Replace(".", ",")); }
            catch (Exception) { Console.WriteLine(text); }
            return res;
        }

        private void start_Click(object sender, EventArgs e)
        {
            recipe recipe = new recipe(
                cleanedText(tot.Text),
                cleanedText(vg.Text),
                cleanedText(pg.Text),
                cleanedText(wat.Text),
                cleanedText(nic.Text),
                null);
            if (ar1_l1.Text != "" && ar1.Text != "") 
                recipe.aroma[ar1_l1.Text] = (float)Convert.ToDouble(ar1.Text.Replace(".",","));
            if (ar2_l1.Text != "" && ar2.Text != "") 
                recipe.aroma[ar2_l1.Text] = (float)Convert.ToDouble(ar2.Text.Replace(".", ","));
            if (ar3_l1.Text != "" && ar3.Text != "") 
                recipe.aroma[ar3_l1.Text] = (float)Convert.ToDouble(ar3.Text.Replace(".", ","));
            if (ar4_l1.Text != "" && ar4.Text != "") 
                recipe.aroma[ar4_l1.Text] = (float)Convert.ToDouble(ar4.Text.Replace(".", ","));

            ingredients baseline = getBaseline();

            ingredients mix = Mix(recipe, baseline);

            tot1.Text = Convert.ToString(mix.vg) + "ml";
            tot2.Text = Convert.ToString(mix.pg) + "ml";
            tot3.Text = Convert.ToString(mix.wat) + "ml";
            tot4.Text = Convert.ToString(mix.nic) + "ml";

            add1.Text = Convert.ToString(mix.reqVG) + "ml";
            add2.Text = Convert.ToString(mix.reqPG) + "ml";
            add3.Text = Convert.ToString(mix.reqWat) + "ml";
            add4.Text = Convert.ToString(mix.reqNic) + "ml";

            addg1.Text = Convert.ToString(mix.reqVG_g) + "g";
            addg2.Text = Convert.ToString(mix.reqPG_g) + "g";
            addg3.Text = Convert.ToString(mix.reqWat_g) + "g";
            addg4.Text = Convert.ToString(mix.reqNic_g) + "g";

            e1.Text = Convert.ToString(mix.p_vg) + "€";
            e2.Text = Convert.ToString(mix.p_pg) + "€";
            e3.Text = Convert.ToString(mix.p_wat) + "€";
            e4.Text = Convert.ToString(mix.p_nic) + "€";

            eu1.Text = Convert.ToString(mix.reqp_vg) + "€";
            eu2.Text = Convert.ToString(mix.reqp_pg) + "€";
            eu3.Text = Convert.ToString(mix.reqp_wat) + "€";
            eu4.Text = Convert.ToString(mix.reqp_nic) + "€";

            ar1_l.Text = ar1_l1.Text;
            ar2_l.Text = ar2_l1.Text;
            ar3_l.Text = ar3_l1.Text;
            ar4_l.Text = ar4_l1.Text;

            if (ar1_l1.Text != "" && ar1.Text != "")
            {
                tot5.Text = Convert.ToString(mix.flav[ar1_l1.Text]) + "ml"; 
                add5.Text = Convert.ToString(mix.reqFlav[ar1_l1.Text]) + "ml";
                addg5.Text = Convert.ToString(mix.reqFlav_g[ar1_l1.Text]) + "g";
                e5.Text = Convert.ToString(mix.price[ar1_l1.Text]) + "€";
                eu5.Text = Convert.ToString(mix.reqPrice[ar1_l1.Text]) + "€";
            }
            else
            {
                tot5.Text = "";
                add5.Text = "";
                addg5.Text = "";
                e5.Text = "";
                eu5.Text = "";
            }

            if (ar2_l1.Text != "" && ar2.Text != "")
            {
                tot6.Text = Convert.ToString(mix.flav[ar2_l1.Text]) + "ml";
                add6.Text = Convert.ToString(mix.reqFlav[ar2_l1.Text]) + "ml";
                addg6.Text = Convert.ToString(mix.reqFlav_g[ar2_l1.Text]) + "g";
                e6.Text = Convert.ToString(mix.price[ar2_l1.Text]) + "€";
                eu6.Text = Convert.ToString(mix.reqPrice[ar2_l1.Text]) + "€";
            }
            else
            {
                tot6.Text = "";
                add6.Text = "";
                addg6.Text = "";
                e6.Text = "";
                eu6.Text = "";
            }

            if (ar3_l1.Text != "" && ar3.Text != "")
            {
                tot7.Text = Convert.ToString(mix.flav[ar3_l1.Text]) + "ml";
                add7.Text = Convert.ToString(mix.reqFlav[ar3_l1.Text]) + "ml";
                addg7.Text = Convert.ToString(mix.reqFlav_g[ar3_l1.Text]) + "g";
                e7.Text = Convert.ToString(mix.price[ar3_l1.Text]) + "€";
                eu7.Text = Convert.ToString(mix.reqPrice[ar3_l1.Text]) + "€";
            }
            else
            {
                tot7.Text = "";
                add7.Text = "";
                addg7.Text = "";
                e7.Text = "";
                eu7.Text = "";
            }

            if (ar4_l1.Text != "" && ar4.Text != "")
            {
                tot8.Text = Convert.ToString(mix.flav[ar4_l1.Text]) + "ml";
                add8.Text = Convert.ToString(mix.reqFlav[ar4_l1.Text]) + "ml";
                addg8.Text = Convert.ToString(mix.reqFlav_g[ar4_l1.Text]) + "g";
                e8.Text = Convert.ToString(mix.price[ar4_l1.Text]) + "€";
                eu8.Text = Convert.ToString(mix.reqPrice[ar4_l1.Text]) + "€";
            }
            else
            {
                tot8.Text = "";
                add8.Text = "";
                addg8.Text = "";
                e8.Text = "";
                eu8.Text = "";
            }


            float min = FindMin(recipe, baseline);
            string min_str = Convert.ToString(min) + "ml";
            if (min == -1) min_str = "+ ∞";
            minimum.Text = min_str;
        }

        private ingredients getBaseline()
        {
            List<recipe> recipes = new List<recipe>();
            if(alt11.Text != "" && cleanedText(alt11.Text) != 0)
            {
                recipe rec = new recipe(
                   cleanedText(alt11.Text),
                   cleanedText(alt12.Text),
                   cleanedText(alt13.Text),
                   cleanedText(alt14.Text),
                   cleanedText(alt15.Text),
                   null);
                if (ar1_l1.Text != "" && alt16.Text != "")
                    rec.aroma[ar1_l1.Text] = (float)Convert.ToDouble(alt16.Text.Replace(".", ","));
                if (ar2_l1.Text != "" && alt17.Text != "")
                    rec.aroma[ar2_l1.Text] = (float)Convert.ToDouble(alt17.Text.Replace(".", ","));
                if (ar3_l1.Text != "" && alt18.Text != "")
                    rec.aroma[ar3_l1.Text] = (float)Convert.ToDouble(alt18.Text.Replace(".", ","));
                if (ar4_l1.Text != "" && alt19.Text != "")
                    rec.aroma[ar4_l1.Text] = (float)Convert.ToDouble(alt19.Text.Replace(".", ","));
                recipes.Add(rec);
            }

            if (alt20.Text != "" && cleanedText(alt20.Text) != 0)
            {
                recipe rec2 = new recipe(
                cleanedText(alt20.Text),
                cleanedText(alt21.Text),
                cleanedText(alt22.Text),
                cleanedText(alt23.Text),
                cleanedText(alt24.Text),
                null);
                if (ar1_l1.Text != "" && alt25.Text != "")
                    rec2.aroma[ar1_l1.Text] = (float)Convert.ToDouble(alt25.Text.Replace(".", ","));
                if (ar2_l1.Text != "" && alt26.Text != "")
                    rec2.aroma[ar2_l1.Text] = (float)Convert.ToDouble(alt26.Text.Replace(".", ","));
                if (ar3_l1.Text != "" && alt27.Text != "")
                    rec2.aroma[ar3_l1.Text] = (float)Convert.ToDouble(alt27.Text.Replace(".", ","));
                if (ar4_l1.Text != "" && alt28.Text != "")
                    rec2.aroma[ar4_l1.Text] = (float)Convert.ToDouble(alt28.Text.Replace(".", ","));
                recipes.Add(rec2);
            }

            ingredients output = AddRecipes(recipes);
            return output;
        }
    }

    public class recipe
    {
        public float total = 0;
        public float vg_conc = 0;
        public float pg_conc = 0;
        public float wat_conc = 0;
        public float nic = 0;
        public Dictionary<string, float> aroma = new Dictionary<string, float>();
        public recipe(float total = 0, float vg_conc = 0, float pg_conc = 0, float wat_conc = 0, float nic = 0,
               Dictionary<string, float> aroma = null) 
        {
            this.total = total;
            this.vg_conc = vg_conc;
            this.pg_conc = pg_conc;
            this.wat_conc = wat_conc;
            this.nic = nic;
            if(aroma != null) this.aroma = aroma;
        }
    }
}
