//
//  NPDatePickerViewController.h
//  Unity-iPhone
//
//  Created by Ayyappa J on 02/12/20.
//

#import <UIKit/UIKit.h>

NS_ASSUME_NONNULL_BEGIN

typedef void(^DatePickerFinishCallback)(NSDate* _Nullable, void* tagPtr);

@interface NPDatePickerViewController : UIViewController
- (id)initWithOptions:(UIDatePickerMode) mode withInitialDate:(NSDate*) initialDateTime withMinimumDate:(NSDate*) minimumDate withMaximumDate:(NSDate*) maximumDate withTag:(void*) tagPtr;

- (void) setFinishCallback:(DatePickerFinishCallback) completion;

- (void*) getTag;
@end

NS_ASSUME_NONNULL_END
