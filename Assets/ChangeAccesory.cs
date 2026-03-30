using System;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAccesory : MonoBehaviour
{
    [Header("Modelos 3D (Accesorios)")]
    public GameObject[] accesorios;

    [Header("Colección Desbloqueada")]
    public bool[] desbloqueados;

    [Header("Color de Piel (Cura)")]
    public SkinnedMeshRenderer zombieRenderer;
    public Material materialZombi;
    public Material materialCurado;
    private bool estaCurado = false;

    void Start()
    {
        for (int i = 0; i < accesorios.Length; i++)
        {
            if (accesorios[i] != null) accesorios[i].SetActive(false);
        }
    }

    public void EquiparAccesorioAleatorio()
    {
        if (accesorios == null || accesorios.Length == 0 || desbloqueados.Length != accesorios.Length) return;

        List<int> opcionesValidas = new List<int>();
        for (int i = 0; i < desbloqueados.Length; i++)
        {
            if (desbloqueados[i] == true) opcionesValidas.Add(i);
        }

        if (opcionesValidas.Count == 0) return;

        int indiceAleatorio = UnityEngine.Random.Range(0, opcionesValidas.Count);
        SincronizarVista(opcionesValidas[indiceAleatorio]);
    }

    public void CambiarColorPiel()
    {
        if (zombieRenderer == null) return;
        estaCurado = !estaCurado;
        zombieRenderer.material = estaCurado ? materialCurado : materialZombi;
    }

    public void DesbloquearYEquiparEspecifico(int indice)
    {
        if (accesorios == null || indice < 0 || indice >= desbloqueados.Length) return;
        desbloqueados[indice] = true;
        SincronizarVista(indice);
    }

    public void CurarZombiDefinitivo()
    {
        if (zombieRenderer != null && materialCurado != null)
        {
            estaCurado = true;
            zombieRenderer.material = materialCurado;
        }
    }

    private void SincronizarVista(int indiceAEncender)
    {
        for (int i = 0; i < accesorios.Length; i++)
        {
            if (accesorios[i] != null) accesorios[i].SetActive(i == indiceAEncender);
        }
    }
}