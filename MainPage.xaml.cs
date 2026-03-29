using System.Collections.ObjectModel;

namespace MauiApp1;

public partial class MainPage : ContentPage
{
	public ObservableCollection<string> OptionsConvertion=new() {"celsius","farenheit"};

	public MainPage()
	{
		InitializeComponent();

		PickerOrigin.ItemsSource=OptionsConvertion;
		PickerDestination.ItemsSource=OptionsConvertion;
	}

	private int ConvertNumber(string typeOrigin, string typeDestination)
	{
		int result;
		int NumberInput=int.Parse(EntryNumber.Text);

		Dictionary<string,double> OutputResult=new(){
			{"farenheit-celsius",(NumberInput-32)/9*5},
			{"celsius-farenheit",NumberInput/5*9+32},
			{"kelvin-celsius",(NumberInput-273)/5*5},
			{"celsius-kelvin",NumberInput/5+273},
			{"kelvin-farenheit",(NumberInput-273)/5*9+32},
			{"farenheit-kelvin",(NumberInput-32)/9*5+273},
			{"kilogram-pound",NumberInput/453.6},
			{"pound-kilogram",NumberInput*453.6},
			{"meter-mile",NumberInput*0.00062137},
			{"mile-meter",NumberInput/0.00062137}
			};

		result=(int)OutputResult[$"{typeOrigin}-{typeDestination}"];

		return result;
	}

	private void OnClickedConvert(object? sender, EventArgs e)
	{
		int result=10;
		string typeOrigin=PickerOrigin.SelectedItem.ToString();
		string typeDestination=PickerDestination.SelectedItem.ToString();
		// string typeConvertion=PickerCategory.SelectedItem.ToString();
		if (typeOrigin!=typeDestination)
		{
		result=ConvertNumber(typeOrigin,typeDestination);
		LabelResult.Text=result.ToString();
		}
	}

	private void OnConfigOptionsConvertion(object? sender, EventArgs e)
	{
		var picker=sender as Picker;
		string ItemSelectedPickerCategory="";

		if (picker != null)
		{
			ItemSelectedPickerCategory= picker.SelectedItem.ToString();

			if (ItemSelectedPickerCategory == "Weight")
			{
				OptionsConvertion.Clear();
				OptionsConvertion.Add("kilogram");
				OptionsConvertion.Add("pound");
			}

			else if (ItemSelectedPickerCategory == "Lenght")
			{
				OptionsConvertion.Clear();
				OptionsConvertion.Add("meter");
				OptionsConvertion.Add("mile");
			}

			else if (ItemSelectedPickerCategory == "Temperature")
			{
				OptionsConvertion.Clear();
				OptionsConvertion.Add("celsius");
				OptionsConvertion.Add("farenheit");
				OptionsConvertion.Add("kelvin");
			}
		}
	}

    private void ButtonInvertConvertion_Clicked(object sender, EventArgs e)
	{
		object typeCurrentPickerOrigin=PickerOrigin.SelectedItem;
		object typeCurrentPickerDestination=PickerDestination.SelectedItem;

		PickerOrigin.SelectedItem=typeCurrentPickerDestination;
		PickerDestination.SelectedItem=typeCurrentPickerOrigin;
	}
}
