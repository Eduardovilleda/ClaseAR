using UnityEngine;
using TMPro;
using System.Collections;

public class QuestManager : MonoBehaviour
{
    [Header("Interfaz de Usuario")]
    public GameObject panelDialogo;
    public TextMeshProUGUI textoDialogo;
    public GameObject panelVictoria;

    [Header("Conexión con el Zombi")]
    public GameObject zombiFisico;
    public ChangeAccesory scriptAccesorios;

    [Header("Animadores")]
    public Animator animZombi;
    public Animator animCientifica;
    public Animator animMedico;

    [Header("Destinos (Tus Marcadores)")]
    public Transform marcador2;
    public Transform marcador3;
    public Transform marcador4;
    public Transform marcador5;

    public float velocidad = 0.3f;
    private Transform destinoActual;

    [Header("Progreso de la Historia")]
    public int faseActual = 0;

    void Start()
    {
        if (panelVictoria != null) panelVictoria.SetActive(false);
        if (panelDialogo != null) panelDialogo.SetActive(true);

        if (zombiFisico != null) destinoActual = zombiFisico.transform;
        ActualizarMision();
    }

    void Update()
    {
        if (zombiFisico != null && destinoActual != null)
        {
            float distancia = Vector3.Distance(zombiFisico.transform.position, destinoActual.position);

            if (distancia > 0.15f)
            {
                zombiFisico.transform.position = Vector3.MoveTowards(zombiFisico.transform.position, destinoActual.position, velocidad * Time.deltaTime);

                Vector3 direccion = destinoActual.position - zombiFisico.transform.position;
                direccion.y = 0;
                if (direccion != Vector3.zero) zombiFisico.transform.rotation = Quaternion.LookRotation(direccion);
            }
        }
    }

    public void AvanzarFase(int marcadorAlcanzado)
    {
        if (faseActual == 0 && marcadorAlcanzado == 4)
        {
            faseActual = 1;
            destinoActual = marcador4;
            if (zombiFisico != null) zombiFisico.transform.SetParent(null); // Se independiza
            ActualizarMision();
        }
        else if (faseActual == 1 && marcadorAlcanzado == 2)
        {
            faseActual = 2;
            destinoActual = marcador2;
            StartCoroutine(RutinaNPC(0, animCientifica, "Científica: 'Toma este radio táctico'. Vuelve al Marcador 4."));
        }
        else if (faseActual == 3 && marcadorAlcanzado == 3)
        {
            faseActual = 4;
            destinoActual = marcador3;
            StartCoroutine(RutinaNPC(1, animMedico, "Médico: 'Carga este botiquín'. Vuelve al Marcador 4."));
        }
        else if (faseActual == 5 && marcadorAlcanzado == 5)
        {
            faseActual = 6;
            destinoActual = marcador5;
            StartCoroutine(RutinaFinal());
        }
    }

    IEnumerator RutinaNPC(int idAccesorio, Animator animNPC, string textoRegreso)
    {
        while (Vector3.Distance(zombiFisico.transform.position, destinoActual.position) > 0.15f)
        {
            yield return null;
        }

        scriptAccesorios.DesbloquearYEquiparEspecifico(idAccesorio);
        if (animZombi != null) animZombi.SetTrigger("GanarAccesorio");

        // ¡LA ÚNICA MODIFICACIÓN!: Apagamos el baile de los NPCs. Se quedarán platicando.
        // if(animNPC != null) animNPC.SetTrigger("Bailar"); 

        if (textoDialogo != null) textoDialogo.text = textoRegreso;

        yield return new WaitForSeconds(4f);

        destinoActual = marcador4;

        if (faseActual == 2) faseActual = 3;
        else if (faseActual == 4) faseActual = 5;

        ActualizarMision();
    }

    IEnumerator RutinaFinal()
    {
        while (Vector3.Distance(zombiFisico.transform.position, destinoActual.position) > 0.15f)
        {
            yield return null;
        }

        scriptAccesorios.CurarZombiDefinitivo();
        if (animZombi != null) animZombi.SetTrigger("GanarAccesorio");

        yield return new WaitForSeconds(2f);

        if (panelDialogo != null) panelDialogo.SetActive(false);
        if (panelVictoria != null) panelVictoria.SetActive(true);
    }

    private void ActualizarMision()
    {
        if (textoDialogo == null) return;
        switch (faseActual)
        {
            case 0: textoDialogo.text = "Misión: Ve al Marcador 4 (Centro)."; break;
            case 1: textoDialogo.text = "Busca a la Científica en el Marcador 2."; break;
            case 3: textoDialogo.text = "Busca al Médico en el Marcador 3."; break;
            case 5: textoDialogo.text = "¡Corre al Marcador 5 para recibir la cura definitiva!"; break;
        }
    }
}