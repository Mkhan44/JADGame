//
//  NPDatePickerViewController.m
//  Unity-iPhone
//
//  Created by Ayyappa J on 02/12/20.
//

#import "NPDatePickerViewController.h"
#import "UIViewController+Presentation.h"
#import "UIView+LayoutConstraints.h"

static const unsigned       kToolBarHeight = 40;

@interface NPDatePickerViewController ()

@property (nonatomic, strong) UIView* datePickerContainer;
@property (nonatomic, strong) UIDatePicker* datePicker;
@property (nonatomic, strong) UIToolbar* toolbar;

@property (nonatomic) UIDatePickerMode mode;
@property (nonatomic) NSDate* initialDate;
@property (nonatomic) NSDate* minimumDate;
@property (nonatomic) NSDate* maximumDate;
@property (nonatomic) void* tagPtr;

@property (nonatomic, copy) DatePickerFinishCallback callback;

@end

@implementation NPDatePickerViewController

@synthesize datePickerContainer;
@synthesize datePicker;
@synthesize toolbar;
@synthesize callback;

- (id)initWithOptions:(UIDatePickerMode) mode withInitialDate:(NSDate*) initialDateTime withMinimumDate:(NSDate*) minimumDate withMaximumDate:(NSDate*) maximumDate withTag:(nonnull void *)tagPtr
{
    if ((self = [super init]))
    {
        _mode = mode;
        _initialDate = initialDateTime;
        _minimumDate = minimumDate;
        _maximumDate = maximumDate;
        _tagPtr      = tagPtr;
    }
    
    return self;
}

- (void)viewDidLoad {
    [super viewDidLoad];
    [self setupContents];
}

- (void)viewWillLayoutSubviews
{
    [self setupConstraints];
}

- (void *) getTag
{
    return _tagPtr;
}

- (void) setFinishCallback:(DatePickerFinishCallback) completion;
{
    self.callback = completion;
}

- (void) setupContents {
    
    [self.view setBackgroundColor:[UIColor colorWithWhite:0.5 alpha:0.5]];
    self.datePickerContainer = [[UIView alloc] initWithFrame:CGRectZero];
    self.datePicker = [[UIDatePicker alloc] initWithFrame:CGRectZero];
    

    [self.datePicker setDate:_initialDate];

    
    self.datePicker.datePickerMode = _mode;
    [self.datePicker setMinimumDate:_minimumDate];
    [self.datePicker setMaximumDate:_maximumDate];
    [self.datePicker setBackgroundColor:[UIColor whiteColor]];

    self.toolbar = [[UIToolbar alloc] initWithFrame:CGRectMake(0, 0, self.view.frame.size.width, kToolBarHeight)];//iOS framework issue where if width is not specified, its throwing constraint errors which is unexpected
    
    [toolbar setBarStyle:UIBarStyleDefault];
    UIBarButtonItem *spaceButton = [[UIBarButtonItem alloc] initWithBarButtonSystemItem:UIBarButtonSystemItemFlexibleSpace target:self action:nil];
    UIBarButtonItem *cancelButton =[[UIBarButtonItem alloc] initWithTitle:@"Cancel" style:UIBarButtonItemStylePlain target:self action:@selector(datePickerCancelClicked:)];
    UIBarButtonItem *doneButton =[[UIBarButtonItem alloc] initWithTitle:@"Done" style:UIBarButtonItemStyleDone target:self action:@selector(datePickerDoneClicked:)];
    
    NSArray *items = [NSArray arrayWithObjects:cancelButton, spaceButton,doneButton, nil];
    [toolbar setItems:items];

    [self.datePickerContainer addSubview: datePicker];
    [self.datePickerContainer addSubview: toolbar];
   
    [self.view setBackgroundColor:[UIColor clearColor]];
    [self.view addSubview:self.datePickerContainer];
    
    [self setupConstraints];
}

- (void)setupConstraints
{
    
    // Setting constraints for the container - Move this to a custom view controller
    [self.datePickerContainer setTranslatesAutoresizingMaskIntoConstraints:FALSE];
    
    CGSize screenSize = [UIScreen mainScreen].bounds.size;
    float maxSize = screenSize.width > screenSize.height ? screenSize.height : screenSize.width;
    [self.datePickerContainer.centerXAnchor constraintEqualToAnchor:self.view.centerXAnchor].active = TRUE;
    [self.datePickerContainer.bottomAnchor constraintEqualToAnchor:self.view.bottomAnchor].active = TRUE;
    if(screenSize.width > screenSize.height) // For having more space when presented in landscape
    {
        [self.datePickerContainer.heightAnchor constraintEqualToAnchor:self.view.heightAnchor multiplier:0.6].active = TRUE;
    }
    else
    {
        [self.datePickerContainer.heightAnchor constraintEqualToAnchor:self.view.heightAnchor multiplier:0.4].active = TRUE;
    }
    [self.datePickerContainer.widthAnchor constraintEqualToConstant:maxSize].active = TRUE;

    [self.toolbar setTranslatesAutoresizingMaskIntoConstraints:FALSE];
    [self.toolbar.topAnchor constraintEqualToAnchor:self.datePickerContainer.topAnchor].active = TRUE;
    [self.toolbar.leadingAnchor constraintEqualToAnchor:self.datePickerContainer.leadingAnchor].active = TRUE;
    [self.toolbar.trailingAnchor constraintEqualToAnchor:self.datePickerContainer.trailingAnchor].active = TRUE;
    [self.toolbar.heightAnchor constraintEqualToConstant:kToolBarHeight].active = TRUE;

    [self.datePicker setTranslatesAutoresizingMaskIntoConstraints:FALSE];
    [self.datePicker.topAnchor constraintEqualToAnchor:self.toolbar.bottomAnchor].active = TRUE;
    [self.datePicker.leadingAnchor constraintEqualToAnchor:self.datePickerContainer.leadingAnchor].active = TRUE;
    [self.datePicker.trailingAnchor constraintEqualToAnchor:self.datePickerContainer.trailingAnchor].active = TRUE;
    [self.datePicker.bottomAnchor constraintEqualToAnchor:self.datePickerContainer.bottomAnchor].active = TRUE;
}

#pragma mark - Callback methods

- (void)datePickerCancelClicked:(id) sender
{
    NSLog(@"Date picker cancel clicked %@ ", sender);
    [self dismissViewControllerAnimated:true completion:nil];
    
    if(self.callback != nil)
    {
        callback(nil, _tagPtr);
    }
}

- (void)datePickerDoneClicked:(id) sender
{
    NSLog(@"Date picker done clicked %@ ", sender);
    [self dismissViewControllerAnimated:true completion:nil];
    
    if(self.callback != nil)
    {
        callback([datePicker date], _tagPtr);
    }
}

@end
