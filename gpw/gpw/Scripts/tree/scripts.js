'use strict';
var tpAllTree = AllTree;
(function($){

    $(function() {

        var datascource = tpAllTree;
        //    {
        //    'id': '0', 'name': 'Nguyễn Viết Kim', 'title': 'Cụ tổ2',
        //    'children': [
        //      {
        //          'id': '1', 'name': 'Nguyễn Viết A',
        //          'children': [
        //          { 'id': '14', 'name': 'Nguyễn Viết A1' },
        //          { 'id': '15', 'name': 'Nguyễn Viết A2' },
        //          { 'id': '16', 'name': 'Nguyễn Viết A3' },
        //          ],
        //      },
        //      { 'id': '2', 'name': 'Nguyễn Viết B' },
        //      {
        //          'id': '3', 'name': 'Nguyễn Viết C',
        //          'children': [
        //          { 'id': '4', 'name': 'Nguyễn Viết C11' },
        //          { 'id': '5', 'name': 'Nguyễn Viết C2' },
        //          { 'id': '6', 'name': 'Nguyễn Viết C3' },
        //          ],
        //      }
        //    ]
        //};
        //datascource ={
        //    'id': '1', 'name': 'Nguyễn Văn A', 'title': 'Trần Thị B',
        //        'children': [
        //             {'id': '2', 'name': 'Nguyễn Văn B', 'title': 'Lê Thị C',
        //                 'children': [
        //                     {'id': '3', 'name': 'Nguyễn Văn B1', 'title': '',
        //                         'children': [
        //                             {'id': '4', 'name': 'Trần Thị Lê', 'title': 'Đỗ Bình',},
        //                         ]},
        //                 ]
        //             },
        //             {
        //                 'id': '5', 'name': 'Nguyễn Văn C', 'title': '',
        //                 'children': [
        //                     {'id': '6', 'name': 'Nguyễn Văn C1', 'title': 'Phạm Ngân',},
        //                 ]
        //             },
        //            {'id': '7', 'name': 'Nguyễn Văn D', 'title': '',
        //                'children': [
        //                    {'id': '8', 'name': 'Nguyễn Văn D1', 'title': 'Hoàng Ngọc222',},
        //                ]
        //            }
        //         ],
        //   };
            
        //datascource ={
        //'name': 'Lao Lao',
        //'title': 'general manager',
        //'children': [
        //  { 'name': 'Bo Miao', 'title': 'department manager' },
        //  { 'name': 'Su Miao', 'title': 'department manager',
        //      'children': [
        //        { 'name': 'Tie Hua', 'title': 'senior engineer' },
        //        { 'name': 'Hei Hei', 'title': 'senior engineer' }
        //      ]
        //  },
        //  { 'name': 'Hong Miao', 'title': 'department manager' },
        //  { 'name': 'Chun Miao', 'title': 'department manager' }
        //]
        //};
    
    var getId = function() {
      return (new Date().getTime()) * 1000 + Math.floor(Math.random() * 1001);
    };

    $('#chart-container').orgchart({
      'data' : datascource,
      'exportButton': true,
      'exportFilename': 'SportsChart',
      'parentNodeSymbol': 'fa-th-large',
      'nodeContent': 'title',
      'createNode': function($node, data) {
        $node[0].id = getId();
      }
    })
    .on('click', '.node', function() {
      var $this = $(this);
      $('#selected-node').val($this.find('.title').text()).data('node', $this);
    })
    .on('click', '.orgchart', function(event) {
      if (!$(event.target).closest('.node').length) {
        $('#selected-node').val('');
      }
    });

    $('input[name="chart-state"]').on('click', function() {
      $('.orgchart').toggleClass('view-state', this.value !== 'view');
      $('#edit-panel').toggleClass('view-state', this.value === 'view');
      if ($(this).val() === 'edit') {
        $('.orgchart').find('tr').removeClass('hidden')
          .find('td').removeClass('hidden')
          .find('.node').removeClass('slide-up slide-down slide-right slide-left');
      } else {
        $('#btn-reset').trigger('click');
      }
    });

    $('input[name="node-type"]').on('click', function() {
      var $this = $(this);
      if ($this.val() === 'parent') {
        $('#edit-panel').addClass('edit-parent-node');
        $('#new-nodelist').children(':gt(0)').remove();
      } else {
        $('#edit-panel').removeClass('edit-parent-node');
      }
    });

    $('#btn-add-input').on('click', function() {
      $('#new-nodelist').append('<li><input type="text" class="new-node"></li>');
    });

    $('#btn-remove-input').on('click', function() {
      var inputs = $('#new-nodelist').children('li');
      if (inputs.length > 1) {
        inputs.last().remove();
      }
    });

    $('#btn-add-nodes').on('click', function() {
      var $chartContainer = $('#chart-container');
      var nodeVals = [];
      $('#new-nodelist').find('.new-node').each(function(index, item) {
        var validVal = item.value.trim();
        if (validVal.length) {
          nodeVals.push(validVal);
        }
      });
      var $node = $('#selected-node').data('node');
      if (!nodeVals.length) {
        alert('Nhập giá trị cho nút mới này');
        return;
      }
      var nodeType = $('input[name="node-type"]:checked');
      if (!nodeType.length) {
        alert('Please select a node type');
        return;
      }
      if (nodeType.val() !== 'parent' && !$('.orgchart').length) {
        alert('Tạo nút gốc trước khi bạn muốn xây dựng cây gia phả từ đầu!');
        return;
      }
      if (nodeType.val() !== 'parent' && !$node) {
        alert('Bạn phải chọn một nút nào đó trong cây gia phả');
        return;
      }
      if (nodeType.val() === 'parent') {
        if (!$chartContainer.children().length) {// if the original chart has been deleted
          $chartContainer.orgchart({
            'data' : { 'name': nodeVals[0] },
            'exportButton': true,
            'exportFilename': 'SportsChart',
            'parentNodeSymbol': 'fa-th-large',
            'createNode': function($node, data) {
              $node[0].id = getId();
            }
          })
          .find('.orgchart').addClass('view-state');
        } else {
          $chartContainer.orgchart('addParent', $chartContainer.find('.node:first'), { 'name': nodeVals[0], 'Id': getId() });
        }
      } else if (nodeType.val() === 'siblings') {
        $chartContainer.orgchart('addSiblings', $node,
          { 'siblings': nodeVals.map(function(item) { return { 'name': item, 'relationship': '110', 'Id': getId() }; })
        });
      } else {
        var hasChild = $node.parent().attr('colspan') > 0 ? true : false;
        if (!hasChild) {
          var rel = nodeVals.length > 1 ? '110' : '100';
          $chartContainer.orgchart('addChildren', $node, {
              'children': nodeVals.map(function(item) {
                return { 'name': item, 'relationship': rel, 'Id': getId() };
              })
            }, $.extend({}, $chartContainer.find('.orgchart').data('options'), { depth: 0 }));
        } else {
          $chartContainer.orgchart('addSiblings', $node.closest('tr').siblings('.nodes').find('.node:first'),
            { 'siblings': nodeVals.map(function(item) { return { 'name': item, 'relationship': '110', 'Id': getId() }; })
          });
        }
      }
    });

    $('#btn-delete-nodes').on('click', function() {
      var $node = $('#selected-node').data('node');
      if (!$node) {
        alert('Please select one node in orgchart');
        return;
      } else if ($node[0] === $('.orgchart').find('.node:first')[0]) {
        if (!window.confirm('Are you sure you want to delete the whole chart?')) {
          return;
        }
      }
      $('#chart-container').orgchart('removeNodes', $node);
      $('#selected-node').val('').data('node', null);
    });

    $('#btn-reset').on('click', function() {
      $('.orgchart').find('.focused').removeClass('focused');
      $('#selected-node').val('');
      $('#new-nodelist').find('input:first').val('').parent().siblings().remove();
      $('#node-type-panel').find('input').prop('checked', false);
    });

  });

})(jQuery);