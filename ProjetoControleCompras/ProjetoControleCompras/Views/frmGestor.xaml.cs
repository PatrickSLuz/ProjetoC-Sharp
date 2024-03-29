﻿using ProjetoControleCompras.DAL;
using ProjetoControleCompras.Models;
using ProjetoControleCompras.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProjetoControleCompras.Views
{
    /// <summary>
    /// Interaction logic for frmGestor.xaml
    /// </summary>
    public partial class frmGestor : Window
    {
        private Agente AgenteLogado;
        public frmGestor(Object agenteLogado)
        {
            InitializeComponent();
            AgenteLogado = (Agente)agenteLogado;
            Atualizar_dtaPedidosValidados_PorSetorEStatus();
            Atualizar_dtaPedidoParaValidar_PorSetorEStatus(0);
            if (AgenteLogado.Setor.NomeSetor.Equals("Financeiro"))
            {
                // Gestor do Setor Financeiro
                btnCadOrcamento.Visibility = Visibility.Visible;
                btnVerificarCompras.Visibility = Visibility.Hidden;
            }
            else if (AgenteLogado.Setor.NomeSetor.Equals("Compras"))
            {
                // Gestor do Setor Compras
                btnCadOrcamento.Visibility = Visibility.Hidden;
                btnVerificarCompras.Visibility = Visibility.Visible;
            }
            else
            {
                // Outro Gestor
                btnCadOrcamento.Visibility = Visibility.Hidden;
                btnVerificarCompras.Visibility = Visibility.Hidden;
            }   
        }

        private void Atualizar_dtaPedidosValidados_PorSetorEStatus()
        {
            List<Pedido> listP = new List<Pedido>();
            foreach (Pedido p in PedidoDAO.ListarPedidosPorSetor(AgenteLogado.Setor.IdSetor))
            {
                if (p.Status.Equals(Status.GetStatus(3))/*Pedido Cancelado*/
                 || p.Status.Equals(Status.GetStatus(4))/*Pedido Finalizado*/)
                {
                    listP.Add(p);
                }
            }
            dtaPedidosValidados.ItemsSource = listP;
        }

        private void Atualizar_dtaPedidoParaValidar_PorSetorEStatus(int index)
        {
            dtaPedidoParaValidar.ItemsSource = PedidoDAO.ListarPedidosPorSetorEStatusIgual(AgenteLogado.Setor.IdSetor, Status.GetStatus(index));
        }

        private void BtnGerenciarAgentes_Click(object sender, RoutedEventArgs e)
        {
            frmGerenciarAgente telaGerenciarAgente = new frmGerenciarAgente(AgenteLogado);
            telaGerenciarAgente.ShowDialog();
        }

        private void BtnValidarPedido_Click(object sender, RoutedEventArgs e)
        {
            dynamic d = dtaPedidoParaValidar.SelectedItem;
            if (d == null)
            {
                MessageBox.Show("Por Favor, Selecione um Pedido para Validar.", "Tela Gestor", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                d.Status = Status.GetStatus(1);
                if (PedidoDAO.AtualizarStatusPedido(d))
                {
                    MessageBox.Show("Pedido Validado com Sucesso.", "Tela Gestor", MessageBoxButton.OK, MessageBoxImage.Information);
                    Atualizar_dtaPedidoParaValidar_PorSetorEStatus(0);
                    Atualizar_dtaPedidosValidados_PorSetorEStatus();
                }
                else
                    MessageBox.Show("Houve um Erro ao Validar o Pedido!", "Tela Gestor", MessageBoxButton.OK, MessageBoxImage.Error);
                
            }
        }

        private void BtnCadOrcamento_Click(object sender, RoutedEventArgs e)
        {
            frmGerenciarOrcamento telaGerenciarOrcamento = new frmGerenciarOrcamento();
            telaGerenciarOrcamento.ShowDialog();
        }

        private void BtnRejeitarPedido_Click(object sender, RoutedEventArgs e)
        {
            dynamic d = dtaPedidoParaValidar.SelectedItem;
            if (d == null)
            {
                MessageBox.Show("Por Favor, Selecione o Pedido que Deseja Rejeitar.", "Tela Gestor", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                d.Status = Status.GetStatus(3);
                if (PedidoDAO.AtualizarStatusPedido(d))
                {
                    MessageBox.Show("Pedido Rejeitado com Sucesso.", "Tela Gestor", MessageBoxButton.OK, MessageBoxImage.Information);
                    Atualizar_dtaPedidosValidados_PorSetorEStatus();
                }
                else
                    MessageBox.Show("Houve um Erro ao Rejeitar o Pedido!", "Tela Gestor", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void BtnVerificarCompras_Click(object sender, RoutedEventArgs e)
        {
            frmGerenciarCompras telaGerenciarCompras = new frmGerenciarCompras();
            telaGerenciarCompras.ShowDialog();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Ver Itens
            AbrirTelaVisualizacao.AbrirTelaVerItens(dtaPedidoParaValidar.SelectedItem);
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            // Ver Orçamentos
            AbrirTelaVisualizacao.AbrirTelaVerOrcamentos(dtaPedidosValidados.SelectedItem);
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            // Ver Itens
            AbrirTelaVisualizacao.AbrirTelaVerItens(dtaPedidoParaValidar.SelectedItem);
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            // Ver Orçamentos
            AbrirTelaVisualizacao.AbrirTelaVerOrcamentos(dtaPedidosValidados.SelectedItem);
        }
    }
}
