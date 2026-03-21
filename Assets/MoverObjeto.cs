using System.Collections;
using UnityEngine;
using Vuforia;

public class MoverObjeto : MonoBehaviour
{
    public Transform modelo;
    public ObserverBehaviour[] marcadores;
    public float velocidad = 0.5f;
    public Animator animador;
    private bool estaMoviendo = false;

    void Start()
    {
        // Pausa suave al inicio
        if (animador != null) animador.speed = 0f;
    }

    public void MoverAlSiguienteMarcador()
    {
        if (!estaMoviendo) StartCoroutine(CorrutinaMovimiento());
    }

    private IEnumerator CorrutinaMovimiento()
    {
        estaMoviendo = true;
        Transform objetivo = ObtenerSiguienteMarcador();

        if (objetivo != null)
        {
            // ¡Play a la animación!
            if (animador != null) animador.speed = 1f;

            Vector3 posicionInicial = modelo.position;
            Vector3 posicionFinal = objetivo.position;
            float tiempo = 0f;

            while (tiempo < 1f)
            {
                tiempo += Time.deltaTime * velocidad;
                modelo.position = Vector3.Lerp(posicionInicial, posicionFinal, tiempo);
                yield return null;
            }

            // Pausa exacta al llegar
            if (animador != null) animador.speed = 0f;
        }

        estaMoviendo = false;
    }

    private Transform ObtenerSiguienteMarcador()
    {
        ObserverBehaviour marcadorDestino = marcadores[1];
        if (marcadorDestino != null && (marcadorDestino.TargetStatus.Status == Status.TRACKED || marcadorDestino.TargetStatus.Status == Status.EXTENDED_TRACKED))
        {
            return marcadorDestino.transform;
        }
        return null;
    }
}