using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class Painting : MonoBehaviour
{
    public Color32[] ColorsOnPainting;
    public Material[] Brushes;
    public int chosenBrush;
    public bool RandomBrush = true;

    private ParticleSystem particles
    {
        get
        {
            if (_particles == null)
            {
                _particles = this.GetComponent<ParticleSystem>();
            }

            return _particles;
        }
    }
    private ParticleSystem _particles;

    void Start()
    {
        recreate();
    }

    public void OnRecreate()
    {
        recreate();
    }

    public void recreate()
    {
        particles.Stop();
        if (this.Brushes != null)
        {
            if (RandomBrush)
            {
                chosenBrush = -1;
                do
                {
                    chosenBrush = setBrush();
                } while (chosenBrush < 0);
            }


            particles.GetComponent<Renderer>().material = Brushes[chosenBrush];
        }
        particles.Play();
        setColors();
        particles.Pause();
    }

    private int setBrush()
    {
        int n = -1;
        n = Random.Range(0, Brushes.Length);
        string s = PlayerPrefs.GetString("Brush" + n.ToString());
        if (s == "true")
            return n;
        else
            return 0;
        //return -1;
    }

    /// <summary>
    /// Uses ColorsOnPainting evenly accross all particles in this particle system.
    /// http://answers.unity3d.com/questions/347675/how-to-change-particle-systems-color-over-lifetime.html
    /// </summary>
    private void setColors()
    {
        ParticleSystem.Particle[] ParticleList = new ParticleSystem.Particle[particles.particleCount];
        particles.GetParticles(ParticleList);
        for (int i = 0; i < ParticleList.Length; ++i)
        {
            //cycle through available colors
            ParticleList[i].color = ColorsOnPainting[i % ColorsOnPainting.Length];
        }

        particles.SetParticles(ParticleList, particles.particleCount);
    }
}
