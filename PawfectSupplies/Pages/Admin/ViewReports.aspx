<%@ Page Title="View Reports" Language="C#" MasterPageFile="~/MasterPages/AdminMaster.master"
    AutoEventWireup="true" CodeBehind="ViewReports.aspx.cs" Inherits="PawfectSupplies.Pages.Admin.ViewReports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="max-w-3xl mx-auto px-6 py-8">
        <h1 class="text-3xl font-bold text-white text-center bg-teal-600 py-4 rounded-md shadow-md">
            📊 Sales & User Activity Reports
        </h1>

        <!-- Grid layout for charts -->
        <div class="grid grid-cols-1 gap-6 mt-6">
            <!-- Total Revenue Over Time -->
            <div class="bg-white shadow-lg rounded-lg p-6">
                <h2 class="text-lg font-semibold text-gray-700 mb-3">💰 Total Revenue Over Time</h2>
                <div class="w-full h-80">
                    <canvas id="RevenueChart"></canvas>
                </div>
            </div>

            <!-- Top Selling Products -->
            <div class="bg-white shadow-lg rounded-lg p-6">
                <h2 class="text-lg font-semibold text-gray-700 mb-3">🔥 Top-Selling Products</h2>
                <div class="w-full h-96">
                    <canvas id="TopSellingChart"></canvas>
                </div>
            </div>

            <!-- Sales Distribution by Category -->
            <div class="bg-white shadow-lg rounded-lg p-6">
                <h2 class="text-lg font-semibold text-gray-700 mb-3">📦 Sales Distribution by Category</h2>
                <div class="w-full h-80">
                    <canvas id="SalesDistributionChart"></canvas>
                </div>
            </div>

            <!-- New User Registrations Over Time -->
            <div class="bg-white shadow-lg rounded-lg p-6">
                <h2 class="text-lg font-semibold text-gray-700 mb-3">👤 New User Registrations Over Time</h2>
                <div class="w-full h-80">
                    <canvas id="UserRegistrationsChart"></canvas>
                </div>
            </div>
        </div>

        <!-- 🔙 Back to Dashboard Button -->
        <div class="mt-8 flex justify-center">
            <a href="AdminDashboard.aspx" class="btn btn-primary bg-gray-700 text-white px-6 py-2 rounded-md shadow-md transition hover:bg-gray-800 hover:scale-105">
                ← Back to Dashboard
            </a>
        </div>
    </div>

    <!-- Chart.js -->
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>

    <script>
        async function loadChartData(url, chartId, chartType, chartLabel, colors) {
            try {
                const response = await fetch(url, { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: '{}' });
                let data = await response.json();
                if (data.d) data = JSON.parse(data.d); // Fix ASP.NET WebMethod JSON response

                if (!data.labels || !data.values) {
                    console.error(`Error: Missing labels/values for ${chartId}`);
                    return;
                }

                const ctx = document.getElementById(chartId).getContext('2d');
                new Chart(ctx, {
                    type: chartType,
                    data: {
                        labels: data.labels,
                        datasets: [{
                            label: chartLabel,
                            data: data.values,
                            backgroundColor: colors,
                            borderColor: '#333',
                            borderWidth: 1
                        }]
                    },
                    options: {
                        responsive: true,
                        maintainAspectRatio: false,
                        plugins: {
                            legend: { display: true, position: 'top' },
                            tooltip: { enabled: true },
                        },
                        scales: chartType === 'bar' || chartType === 'line' ? {
                            y: { beginAtZero: true },
                            x: {
                                ticks: {
                                    maxRotation: 25, // Auto-adjust labels for better readability
                                    minRotation: 25,
                                    autoSkip: false
                                }
                            }
                        } : {}
                    }
                });
            } catch (error) {
                console.error(`Failed to load ${chartId}:`, error);
            }
        }

        document.addEventListener('DOMContentLoaded', function () {
            loadChartData('ViewReports.aspx/GetRevenueData', 'RevenueChart', 'line', 'Total Revenue ($)', ['rgba(23, 162, 184, 0.5)']);
            loadChartData('ViewReports.aspx/GetTopSellingProducts', 'TopSellingChart', 'bar', 'Top-Selling Products', ['#FF5733', '#FFC107', '#2196F3', '#9C27B0', '#4CAF50']);
            loadChartData('ViewReports.aspx/GetSalesDistribution', 'SalesDistributionChart', 'pie', 'Sales Distribution', ['#4CAF50', '#FF5733', '#FFC107', '#2196F3']);
            loadChartData('ViewReports.aspx/GetUserRegistrations', 'UserRegistrationsChart', 'line', 'New Users Per Month', ['rgba(0, 128, 255, 0.6)']);
        });
    </script>
</asp:Content>


